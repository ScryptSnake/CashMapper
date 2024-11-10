using CashMapper.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using TestProject.Fixtures;


namespace TestProject.Fixtures;

// A fixture for tests. Connects to database and exposes repository properties for test cases.
public class RepositoryFixture : IDisposable
{

    private const string databasePath =
        @"C:\Users\WIN11PC\source\repos\CashMapper\TestProject\Resources\test_database.db";
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
        var settings = new DatabaseSettings() { Path = databasePath };
        var options = Options.Create(settings);

        // Get a factory.
        var dbFactory = new DatabaseFactory(options);

        // Instantiate Repos.
        IncomeItemRepo = new IncomeItemRepository(dbFactory);
        BudgetItemRepo = new BudgetItemRepository(dbFactory);
        CashflowEntryRepo = new CashflowEntryRepository(dbFactory);
        TransactionRepo = new TransactionRepository(dbFactory);
        IncomeProfileRepo = new IncomeProfileRepository(dbFactory);
        ExpenseItemRepo = new ExpenseItemRepository(dbFactory);
        AccountProfileRepo = new AccountProfileRepository(dbFactory);
        CategoryRepo = new CategoryRepository(dbFactory);
    }

    public void Dispose()
    {
        // ... clean up test data from the database ...

    }

}


