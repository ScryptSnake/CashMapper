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
/// Provides methods for performing CRUD operations on the categories table. 
/// </summary>
public class CategoryRepository : IRepository<Category>
{
    private Task<IDatabase> DatabaseTask { get; }

    public CategoryRepository(IDatabaseFactory databaseFactory){
        // Get the database from factory, store the task.
        DatabaseTask = databaseFactory.GetDatabase();
    }

    public async Task<bool> ExistsAsync(Category entity)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM categories WHERE id=@Id;";
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

    public async Task<Category> FindAsync(long id)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT id, name, category_type,
                           date_created, date_modified, flag
                           FROM categories WHERE id=@id;";
        var entity = await db.GetAsync<Category>(SQL, new { id });
        return entity;
    }

    public async Task<Category> GetAsync(Category entity)
    {
        return await FindAsync(entity.Id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        const string sql = @$"SELECT id, name, category_type,
                    date_created, date_modified, flag 
                    FROM categories;";
        var db = await DatabaseTask;
        return await db.GetMultipleAsync<Category>(sql);
    }

    public async Task<Category> AddAsync(Category entity)
    {
        // Note:  DateCreated and DateModified fields default to current timestamp inside backend.
        const string SQL = @$"INSERT INTO categories(name, category_type, flag)
                            VALUES(@Name, @CategoryType, @Flag);
                            SELECT last_insert_rowId();";
        var db = await DatabaseTask;
        var id = await db.ExecuteScalarAsync<long>(SQL, entity);
        return await FindAsync(id);
    }

    public async Task<Category> UpdateAsync(Category entity)
    {
        if (entity.Id == default) throw new InvalidDataException("Category Id field not provided.");
        var sql = $@"UPDATE categories
                  SET name=@Name, category_type=@CategoryType, flag=@Flag,
                  date_modified='{DateTimeOffset.Now.UtcDateTime.ToString("s", CultureInfo.InvariantCulture)}'
                  WHERE id=@Id;";
        var db = await DatabaseTask;
        await db.ExecuteAsync(sql, entity);
        return await FindAsync(entity.Id);
    }

    public async Task<Category> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentNullException("Category name required.");

        const string SQL = @$"SELECT id, name, category_type,
                    date_created, date_modified, flag 
                    FROM categories WHERE name=@name;";
        var db = await DatabaseTask;
        return await db.GetAsync<Category>(SQL, new { name });
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        var db = await DatabaseTask;
        const string SQL = @"SELECT COUNT(id) FROM categories WHERE name=@name;";
        var count = await db.ExecuteScalarAsync<long>(SQL, new { name });
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

}
