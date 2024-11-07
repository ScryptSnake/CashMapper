using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;
using Dapper;


namespace CashMapper.DataAccess;

public class IncomeItemRepository: IRepository<IncomeItem>
{
    //Name of the sqlite database table:
    private const string TableName = "income_items";
    private Task<IDatabase> DatabaseTask { get; }


    public IncomeItemRepository(IDatabaseFactory databaseFactory) //this would be the factory instead. Not the DB instance.
    {
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(IncomeItem entity)
    {
        var db = await DatabaseTask;
        var sql = $"SELECT COUNT(id) FROM {TableName} WHERE id=@id;";
        var count = await db.ExecuteScalarAsync<long>(sql, entity);
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
        var sql = $"SELECT * FROM {TableName} WHERE id=@id;";
        var entity = await db.GetAsync<IncomeItem>(sql, new { id = id });
        return entity;
    }

    public async Task<IncomeItem> GetAsync(IncomeItem entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<IncomeItem>> GetMultipleAsync(QueryFilter filter)
    {
        if (filter.IsEmpty()) throw new InvalidDataException("Filter provided is empty.");
        var sql = $"SELECT * FROM {TableName} WHERE {filter.ToString()};";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<IncomeItem>(sql, filter.GetParameter());

    }

    public async Task<IncomeItem> AddAsync(IncomeItem entity)
    {
        var sql = @$"INSERT INTO {TableName}(name,income_profile_id,monthly_value)
                    VALUES(@Name,@IncomeProfileId,@MonthlyValue);
                    SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(sql, entity);
        return await FindAsync(id);
    }

    public Task<IncomeItem> UpdateAsync(IncomeItem entity)
    {
        throw new NotImplementedException();
    }
}
