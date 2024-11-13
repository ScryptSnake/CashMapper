using CashMapper.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Bson;
using TestProject.Fixtures;


namespace TestProject.Fixtures;

// A fixture for tests. Connects to database and exposes repository properties for test cases.
public class RepositoryFixture : IDisposable
{

    private const string DATABASE_PATH =
        @"C:\Users\WIN11PC\source\repos\CashMapper\TestProject\Resources\test_database.db";

    private IDatabaseFactory DatabaseFactory { get; }

    public IRepository<IncomeItem> IncomeItemRepo { get; }
    public IRepository<BudgetItem> BudgetItemRepo { get; }
    public IRepository<CashflowEntry> CashflowEntryRepo { get; }
    public IRepository<Transaction> TransactionRepo { get; }
    public IRepository<IncomeProfile> IncomeProfileRepo { get; }
    public IRepository<ExpenseItem> ExpenseItemRepo { get; }
    public IRepository<AccountProfile> AccountProfileRepo { get; }
    public IRepository<Category> CategoryRepo { get; }

    public RepositoryFixture()
    {

        // Create DB settings.
        var settings = new DatabaseSettings() { Path = DATABASE_PATH };
        var options = Options.Create(settings);

        // Get a factory.
        DatabaseFactory = new DatabaseFactory(options);

        // Instantiate Repos.
        IncomeItemRepo = new IncomeItemRepository(DatabaseFactory);
        BudgetItemRepo = new BudgetItemRepository(DatabaseFactory);
        CashflowEntryRepo = new CashflowEntryRepository(DatabaseFactory);
        TransactionRepo = new TransactionRepository(DatabaseFactory);
        IncomeProfileRepo = new IncomeProfileRepository(DatabaseFactory);
        ExpenseItemRepo = new ExpenseItemRepository(DatabaseFactory);
        AccountProfileRepo = new AccountProfileRepository(DatabaseFactory);
        CategoryRepo = new CategoryRepository(DatabaseFactory);

        // Prepare database for testing.
        Task.Run(() => InitializeDataAsync()).GetAwaiter().GetResult();
    }

    private async Task InitializeDataAsync()
    {
        // Prepares the database for tests.
        BackupDatabase();
        await WipeDatabaseAsync();
    }


    private void BackupDatabase()
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var destination = $"{localAppData}\\database_backup";

        File.Copy(DATABASE_PATH, destination, true);
        
    }

    private async Task WipeDatabaseAsync()
    {
        // Grab database to execute sql. 
        var db = await DatabaseFactory.GetDatabase();

        // Get list of tables. 
        var sql = "SELECT name FROM sqlite_master WHERE type='table';";
        var tables = await db.GetMultipleAsync<string>(sql);

        // Disable constraints.
        await db.ExecuteAsync("PRAGMA foreign_keys = OFF;");


        // Wipe each table. 
        foreach (var table in tables)
        {
            var query = $"DELETE FROM {table};";
            await db.ExecuteAsync(query);
        }
        // Enable constraints.
        await db.ExecuteAsync("PRAGMA foreign_keys = ON;");
    }


    public void Dispose()
    {
        // ... clean up test data from the database ...

    }

}


