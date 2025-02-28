﻿
using System.Diagnostics;
using CashMapper.DataAccess;
using CashMapper.DataAccess.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CashMapper.DataAccess.Repositories;

namespace ConsoleDemoApp;


public class Program
{


    public static async Task Main(string[] args)
    {

        Console.WriteLine("Starting....");

        // Setup DI container.
        var services = RegisterServices();

        // Build a provider.
        var provider = BuildServices(services);

        ////Get category repo service:
        //var catRepo = provider.GetRequiredService<IRepository<Category>>();
        //var category = await catRepo.AddAsync(new Category() { Name = "FUEL9" });
        //category = await catRepo.GetAsync(category);


        //Get repo service
        var repo = provider.GetRequiredService<IRepository<Transaction>>();

        var transactions = repo.GetAllAsync();


        //var newItem = new Transaction()
        //{
        //    Description = "A transaction made to the account!",
        //    Note = "This is where details go.",
        //    Value=4.40484040m, TransactionDate=DateTime.Now,
        //    CategoryId = 2
            
            
        //};
        //var item = await repo.AddAsync(newItem);

        //Console.WriteLine("Complete: " + item.ToString());
        //Console.ReadLine(); // Keep Window Open. 


    }



    private static IServiceProvider BuildServices(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }


    private static IServiceCollection RegisterServices()
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        // Create a new config from appSettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(projectDirectory)
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true) 
            .Build();

        // Create a new DI container to hold services. Register dependencies.
        var services = new ServiceCollection()
            // Add the configuration.
            .AddSingleton<IConfiguration>(config)
            // Create new DB settings instance, bind to the config section. 
            .Configure<DatabaseSettings>(config.GetSection("DatabaseSettings"))
            // Add a DB factory.
            .AddSingleton<IDatabaseFactory, DatabaseFactory>()
            // Add repositories.
            .AddSingleton<IRepository<Category>,CategoryRepository>()
            .AddSingleton<IRepository<IncomeItem>, IncomeItemRepository>()
            .AddSingleton<IRepository<BudgetItem>, BudgetItemRepository>()
            .AddSingleton<IRepository<ExpenseItem>, ExpenseItemRepository>()
            .AddSingleton<IRepository<Transaction>, TransactionRepository>();


        return services;


    }

}