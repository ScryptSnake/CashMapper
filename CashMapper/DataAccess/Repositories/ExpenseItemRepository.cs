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
/// Provides methods for performing CRUD operations on the expense_items table. 
/// </summary>
public class ExpenseItemRepository : IRepository<ExpenseItem>
{
    private Task<IDatabase> DatabaseTask { get; }

    public ExpenseItemRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(ExpenseItem entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM expense_items WHERE id=@id;";
        var count = await db.ExecuteScalarAsync<long>(SQL, entity);
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

    public async Task<ExpenseItem> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, description, monthly_value, note, category_id,
                           category_id, date_created, date_modified, flag
                           FROM expense_items WHERE id=@id;";
        var entity = await db.GetAsync<ExpenseItem>(SQL, new { id });
        return entity;
    }

    public async Task<ExpenseItem> GetAsync(ExpenseItem entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<ExpenseItem>> GetAllAsync()
    {
        const string SQL = @$"SELECT id, description, monthly_value, note, category_id,
                    date_created, date_modified, flag 
                    FROM expense_items;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<ExpenseItem>(SQL);
    }

    public async Task<ExpenseItem> AddAsync(ExpenseItem entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO expense_items(description, monthly_value, note, category_id, flag)
                            VALUES(@Description, @MonthlyValue, @Note, @CategoryId, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<ExpenseItem> UpdateAsync(ExpenseItem entity)
    {
        if (entity.Id == default) throw new InvalidDataException("ExpenseItem Id field not provided.");
        var sql = $@"UPDATE expense_items
                  SET description=@Description, note=@Note,
                  monthly_value=@MonthlyValue, category_id=@CategoryId, flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }
}
