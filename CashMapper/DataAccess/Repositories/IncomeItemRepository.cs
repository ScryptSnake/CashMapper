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
using static Dapper.SqlMapper;


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

    public async Task<bool> ExistsAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM income_items WHERE id=@Id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, new {Id=id});
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

    public async Task<IncomeItem?> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, name, income_profile_id, CAST(monthly_value AS REAL) AS value,
                           date_created, date_modified, flag
                           FROM income_items WHERE id=@id;";
        var entity = await db.GetAsync<IncomeItem>(SQL, new { id });
        return entity;
    }

    public async Task<IncomeItem> GetAsync(IncomeItem entity)
    {
        var result = await FindAsync(entity.Id);
        return result ?? throw new DataException("Provided entity does not exist.");
    }

    public async Task<IEnumerable<IncomeItem>> GetAllAsync()
    {
        const string SQL = @"SELECT id, name, income_profile_id, CAST(monthly_value AS REAL) AS value, 
                            date_created, date_modified, flag 
                            FROM income_items ORDER BY income_profile_id;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<IncomeItem>(SQL);
    }

    public async Task<IncomeItem> AddAsync(IncomeItem entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @"INSERT INTO income_items(name,income_profile_id,monthly_value,flag)
                    VALUES(@Name, @IncomeProfileId, @MonthlyValue, @Flag);
                    SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await GetAsync(entity with {Id=id});
    }

    public async Task<IncomeItem> UpdateAsync(IncomeItem entity)
    {
        if (entity.Id == default) throw new InvalidDataException("IncomeItem Id field not provided.");
        var sql = $@"UPDATE income_items
                  SET name=@Name, income_profile_id=@IncomeProfileId, 
                  monthly_value=@MonthlyValue,flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await GetAsync(entity);
    }

    public async Task<IEnumerable<IncomeItem>> GetByIncomeProfile(IncomeProfile profile)
    {
        if (profile.Id == default) throw new InvalidDataException("Income profile Id field not provided.");
        return await GetByIncomeProfileId(profile.Id);
    }
    public async Task<IEnumerable<IncomeItem>> GetByIncomeProfileId(long profileId)
    {
        const string SQL = $@"SELECT id, name, income_profile_id, 
                            CAST(monthly_value AS REAL) AS value, date_created, date_modified, flag
                            FROM income_items WHERE income_profile_id=@Id 
                            ORDER BY date_created;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<IncomeItem>(SQL,new {Id=profileId});
    }
}
