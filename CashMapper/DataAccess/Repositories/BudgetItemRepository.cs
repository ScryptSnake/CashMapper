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

public class BudgetItemRepository : IRepository<BudgetItem>
{
    private Task<IDatabase> DatabaseTask { get; }

    public BudgetItemRepository(IDatabaseFactory databaseFactory) //this would be the factory instead. Not the DB instance.
    {
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(BudgetItem entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM budget_items WHERE id=@id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, entity);
        switch (count)
        {
            case 0: return false;
            case 1: return true;
            case > 1: throw new DataException("Database returned multiple records. Expected 1 or 0.");
        }
        return false;
    }

    public async Task<BudgetItem> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, description, monthly_value,
                           category_id, date_created, date_modified, flag
                           FROM budget_items WHERE id=@id;";
        var entity = await db.GetAsync<BudgetItem>(SQL, new { id });
        return entity;
    }

    public async Task<BudgetItem> GetAsync(BudgetItem entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<BudgetItem>> GetMultipleAsync(QueryFilter filter)
    {
        if (filter.IsEmpty()) throw new InvalidDataException("Filter provided is empty.");
        var sql = @$"SELECT id, name, income_profile_id, monthly_value, 
                            date_created, date_modified, flag 
                            FROM budget_items WHERE {filter.ToString()};";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<BudgetItem>(sql, filter.GetParameter());
    }

    public async Task<BudgetItem> AddAsync(BudgetItem entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO budget_items(name,income_profile_id,monthly_value)
                    VALUES(@Name,@IncomeProfileId,@MonthlyValue);
                    SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<BudgetItem> UpdateAsync(BudgetItem entity)
    {
        if (entity.Id == default) throw new InvalidDataException("BudgetItem Id field not provided.");
        var sql = $@"UPDATE budget_items
                  SET name=@Name, income_profile_id=@IncomeProfileId, 
                  monthly_value=@MonthlyValue,
                  @date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }
}
