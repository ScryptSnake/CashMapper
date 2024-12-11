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
/// Provides methods for performing CRUD operations on the budget_items table. 
/// </summary>
public class BudgetItemRepository : IRepository<BudgetItem>
{
    private Task<IDatabase> DatabaseTask { get; }

    public BudgetItemRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM budget_items WHERE id=@Id;";
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

    public async Task<BudgetItem?> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, description, CAST(monthly_value AS DECIMAL), note, category_id,
                           date_created, date_modified, flag
                           FROM budget_items WHERE id=@id;";
        var entity = await db.GetAsync<BudgetItem>(SQL, new { id });
        return entity;
    }

    public async Task<BudgetItem> GetAsync(BudgetItem entity)
    {
        var result = await FindAsync(entity.Id);
        return result ?? throw new DataException("Provided entity does not exist.");
    }

    public async Task<IEnumerable<BudgetItem>> GetAllAsync()
    {
        const string SQL = @"SELECT id, description, CAST(monthly_value AS DECIMAL), note, category_id,
                    date_created, date_modified, flag 
                    FROM budget_items;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<BudgetItem>(SQL);
    }

    public async Task<BudgetItem> AddAsync(BudgetItem entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @"INSERT INTO budget_items(description, monthly_value, note, category_id, flag)
                            VALUES(@Description, @MonthlyValue, @Note, @CategoryId, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await GetAsync(entity with { Id = id });
    }

    public async Task<BudgetItem> UpdateAsync(BudgetItem entity)
    {
        if (entity.Id == default) throw new InvalidDataException("BudgetItem Id field not provided.");
        var sql = $@"UPDATE budget_items
                  SET description=@Description, note=@Note, flag=@Flag,
                  monthly_value=@MonthlyValue, category_id=@CategoryId,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await GetAsync(entity);
    }

    public async Task<IEnumerable<BudgetItem>> GetByCategoryId(long categoryId)
    {
        const string SQL = @"SELECT id, description, CAST(monthly_value AS DECIMAL),
                    note, category_id, date_created, date_modified, flag
                    FROM budget_items WHERE category_id=@Id ORDER BY description;";
        var db = await DatabaseTask;
        var result = await db.GetMultipleAsync<BudgetItem>(SQL, new { Id = categoryId });
        return result;
    }

    public async Task<IEnumerable<BudgetItem>> GetByCategoryName(string categoryName)
    {
        var db = await DatabaseTask;
        const string categorySQL = @"SELECT id, name, date_created, date_modified, flag
                                    FROM categories WHERE name=@Name;";
        var category = await db.GetAsync<Category>(categorySQL, new { Name = categoryName });
        if (category == null) return Enumerable.Empty<BudgetItem>();
        return await GetByCategoryId(category.Id);
    }


}
