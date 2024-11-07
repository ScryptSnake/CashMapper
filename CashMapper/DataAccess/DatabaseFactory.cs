using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CashMapper.DataAccess;
/// <summary>
/// A concrete type that returns a new SqliteDatabase
/// </summary>
public class DatabaseFactory : IDatabaseFactory
{
    private readonly IOptions<DatabaseSettings> options;
    private Task Migration { get; }

    public DatabaseFactory(IOptions<DatabaseSettings> dbSettings)
    {
        options = dbSettings;
        // Start migrations, store the task.
        Migration = DatabaseMigrations.Migrate(dbSettings);
    }

    public async Task<IDatabase> GetDatabase()
    {
        // Await migration task to finish. Return new instance of DB.
        await Migration;
        return new SqliteDatabase(options.Value);
    }
}