using System;
using System.Collections;
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
/// Provides methods for performing CRUD operations on the account_profiles table. 
/// </summary>
public class AccountProfileRepository : IRepository<AccountProfile>
{
    private Task<IDatabase> DatabaseTask { get; }

    public AccountProfileRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(AccountProfile entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM account_profiles WHERE id=@Id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, entity);
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            case > 1: throw new DataException("Database returned multiple records. Expected 1 or 0.");
        }
        return false;
    }

    public async Task<AccountProfile> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, name,
                           date_created, date_modified, flag
                           FROM account_profiles WHERE id=@Id;";
        var entity = await db.GetAsync<AccountProfile>(SQL, new { id });
        return entity;
    }

    public async Task<AccountProfile> GetAsync(AccountProfile entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<AccountProfile>> GetAllAsync()
    {
        const string SQL = @"SELECT id, name,
                    date_created, date_modified, flag 
                    FROM account_profiles;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<AccountProfile>(SQL);
    }

    public async Task<AccountProfile> AddAsync(AccountProfile entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO account_profiles(name, flag)
                            VALUES(@Name, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<AccountProfile> UpdateAsync(AccountProfile entity)
    {
        if (entity.Id == default) throw new InvalidDataException("AccountProfile Id field not provided.");
        var sql = $@"UPDATE account_profiles
                  SET description=@name=Name, flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }
}
