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
/// Provides methods for performing CRUD operations on the income_items table. 
/// </summary>
/// <seealso cref="CashMapper.DataAccess.Repositories.IRepository&lt;CashMapper.DataAccess.Entities.IncomeItem&gt;" />
public class IncomeItemRepository : IRepository<IncomeItem>
{
    private Task<IDatabase> DatabaseTask { get; }

    public IncomeItemRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(IncomeItem entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM income_items WHERE id=@id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, entity);
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            case > 1: throw new DataException("Database returned multiple records. Expected 1 or 0.");
        }
        return false;
    }

    public async Task<IncomeItem> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, name, income_profile_id, monthly_value,
                           date_created, date_modified, flag
                           FROM income_items WHERE id=@id;";
        var entity = await db.GetAsync<IncomeItem>(SQL, new { id });
        return entity;
    }

    public async Task<IncomeItem> GetAsync(IncomeItem entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<IncomeItem>> GetAllAsync()
    {
        const string SQL = @"SELECT id, name, income_profile_id, monthly_value, 
                            date_created, date_modified, flag 
                            FROM income_items;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<IncomeItem>(SQL);
    }

    public async Task<IncomeItem> AddAsync(IncomeItem entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @"INSERT INTO income_items(name,income_profile_id,monthly_value)
                    VALUES(@Name, @IncomeProfileId, @MonthlyValue);
                    SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<IncomeItem> UpdateAsync(IncomeItem entity)
    {
        if (entity.Id == default) throw new InvalidDataException("IncomeItem Id field not provided.");
        var sql = $@"UPDATE income_items
                  SET name=@Name, income_profile_id=@IncomeProfileId, 
                  monthly_value=@MonthlyValue,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }
}
