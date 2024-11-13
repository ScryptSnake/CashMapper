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
/// Provides methods for performing CRUD operations on the income_profiles table. 
/// </summary>
public class IncomeProfileRepository : IRepository<IncomeProfile>
{
    private Task<IDatabase> DatabaseTask { get; }
    
    public IncomeProfileRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(IncomeProfile entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM income_profiles WHERE id=@Id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, entity);
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            case > 1: throw new DataException("Database returned multiple records. Expected 1 or 0.");
        }
        return false;
    }

    public async Task<IncomeProfile> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, name,
                           date_created, date_modified, flag
                           FROM income_profiles WHERE id=@id;";
        var entity = await db.GetAsync<IncomeProfile>(SQL, new { id });
        return entity;
    }

    public async Task<IncomeProfile> GetAsync(IncomeProfile entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<IncomeProfile>> GetAllAsync()
    {
        const string SQL = @$"SELECT id, name,
                    date_created, date_modified, flag 
                    FROM income_profiles;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<IncomeProfile>(SQL);
    }

    public async Task<IncomeProfile> AddAsync(IncomeProfile entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO income_profiles(name,flag)
                            VALUES(@Name, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<IncomeProfile> UpdateAsync(IncomeProfile entity)
    {
        if (entity.Id == default) throw new InvalidDataException("IncomeProfile Id field not provided.");
        var sql = $@"UPDATE income_profiles
                  SET name=@Name, flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }
}
