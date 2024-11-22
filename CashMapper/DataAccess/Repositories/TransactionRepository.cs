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
using CashMapper.DataAccess.Filters;
using CashMapper.Enums;
using Dapper;


namespace CashMapper.DataAccess.Repositories;

/// <summary>
/// Provides methods for performing CRUD operations on the transactions table. 
/// </summary>
public class TransactionRepository : IRepository<Transaction>
{
    private Task<IDatabase> DatabaseTask { get; }

    public TransactionRepository(IDatabaseFactory databaseFactory)
    {
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM transactions WHERE id=@Id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, new { Id = id });
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            default:
                throw new DataException(
                    $"""
                     Database returned an invalid number of records.
                      Expected 1 or 0. Actual: {count}
                     """);
        }

        return false;
    }

    public async Task<Transaction?> FindAsync(long id)
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
        var result = await FindAsync(entity.Id);
        return result ?? throw new DataException("Provided entity does not exist.");
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        const string SQL = @$"SELECT id, description, source, date AS transaction_date, value,
                    note, category_id, date_created, date_modified, flag 
                    FROM transactions;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<Transaction>(SQL);
    }

    public async Task<Transaction> AddAsync(Transaction entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO transactions(description, source, date, value, note, category_id, flag)
                            VALUES(@Description, @Source, @TransactionDate, @Value, @Note, @CategoryId, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await GetAsync(entity with { Id = id });
    }

    public async Task<Transaction> UpdateAsync(Transaction entity)
    {
        if (entity.Id == default) throw new InvalidDataException("Transaction Id field not provided.");
        var sql = $@"UPDATE transactions
                  SET description=@Description, note=@Note,
                  source=@Source, date=@TransactionDate, value=@Value, category_id=@CategoryId, flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<Transaction>> Query(TransactionQueryFilter filter)
    {
        var builder = new QueryBuilder();
        builder.AddCriteria("category_id", filter.CategoryId, QueryOperators.Equals);
        builder.AddCriteria("description", filter.DescriptionLike, QueryOperators.Like);
        builder.AddCriteria("source", filter.source, QueryOperators.Equals);
        builder.AddRangeCriteria("value", filter.ValueRange);
        builder.AddRangeCriteria("date", filter.DateRange);
        builder.AddCriteria("note", filter.NoteLike, QueryOperators.Like);
        builder.AddCriteria("flag", filter.Flag, QueryOperators.Equals);

        var sqlWhere = builder.BuildWhereClause(false);
        var paramObj = builder.GetParameter();

        var sql = $@"SELECT id, description, source, date, 
                    value, category_id, note, date_created, date_modified, flag
                    FROM transactions {sqlWhere} ORDER BY date;";

        var db = await DatabaseTask;
        return await db.GetMultipleAsync<Transaction>(sql, paramObj);
    }
}