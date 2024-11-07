
using System.Diagnostics;
using CashMapper.DataAccess;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ConsoleDemoApp;


public class Program
{


    public static async Task Main(string[] args)
    {

        ConfigureServices();





    }


    public static ServiceCollection RegisterServices()
    {
        // Create a new config from appSettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true) 
            .Build();

        // Create a new DI container to hold services. Register dependencies.
        var services = new ServiceCollection()
            // Add the configuration.
            .AddSingleton<IConfiguration>(config)
            // Create new DB settings instance, bind to the config section. 
            .Configure<DatabaseSettings>(config.GetSection("DatabaseSettings"))
            // Add a DB factory.
            .AddSingleton<IDatabaseFactory, DatabaseFactory>();


    }

}