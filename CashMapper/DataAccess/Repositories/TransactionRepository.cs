using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;
using Dapper;


namespace CashMapper.DataAccess.Repositories;

/// <summary>
/// Provides methods for performing CRUD operations on the transactions table. 
/// </summary>
public class TransactionRepository : IRepository<Transaction>
{
    private Task<IDatabase> DatabaseTask { get; }

    public TransactionRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(Transaction entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM transactions WHERE id=@id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, entity);
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            case > 1: throw new DataException("Database returned multiple records. Expected 1 or 0.");
        }
        return false;
    }

    public async Task<Transaction> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, description, source, 
                           date AS transaction_date, value, note, category_id,
                           category_id, date_created, date_modified, flag
                           FROM transactions WHERE id=@id;";
        var entity = await db.GetAsync<Transaction>(SQL, new { id });
        return entity;
    }

    public async Task<Transaction> GetAsync(Transaction entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<Transaction>> GetMultipleAsync(QueryFilter filter)
    {
        if (filter.IsEmpty()) throw new InvalidDataException("Filter provided is empty.");
        var sql = @$"SELECT id, description, source, date AS transaction_date, value,
                    note, category_id, date_created, date_modified, flag 
                    FROM transactions WHERE {filter.ToString()};";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<Transaction>(sql, filter.GetParameter());
    }

    public async Task<Transaction> AddAsync(Transaction entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO transactions(description, source, date, value, note, category_id)
                            VALUES(@Description, @Source, @TransactionDate, @Value, @Note, @CategoryId);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<Transaction> UpdateAsync(Transaction entity)
    {
        if (entity.Id == default) throw new InvalidDataException("Transaction Id field not provided.");
        var sql = $@"UPDATE transactions
                  SET description=@Description, note=@Note,
                  source=@Source, date=@TransactionDate, value=@Value, CategoryId=@category_id,
                  @date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }
}
