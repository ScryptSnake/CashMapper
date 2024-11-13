using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CashMapper.DataAccess
{
    /// <summary>
    /// A class that migrates the SqliteDB schema and maintains current version.
    /// </summary>
    internal static class DatabaseMigrations
    {
        private const int CurrentVersion = 1;
        private const string Version1 = """
                                    CREATE TABLE categories
                                    (
                                        id                  INTEGER PRIMARY KEY,
                                        name                TEXT NOT NULL,
                                        category_type       INTEGER NOT NULL,
                                        date_created        TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified       TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag                TEXT
                                    );
                                    CREATE UNIQUE INDEX u_idx__categories__name ON categories(name);
                                    
                                    
                                    CREATE TABLE account_profiles
                                    (
                                        id              INTEGER PRIMARY KEY,
                                        name            TEXT NOT NULL,
                                        date_created    TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified   TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag            TEXT
                                    );
                                    CREATE UNIQUE INDEX u_idx__acount_profiles__name ON account_profiles(name);
                                    
                                    
                                    
                                    CREATE TABLE income_profiles
                                    (
                                        id              INTEGER PRIMARY KEY,
                                        name            TEXT NOT NULL,
                                        date_created    TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified   TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag            TEXT
                                    );
                                    CREATE UNIQUE INDEX u_idx__income_profiles__name ON income_profiles(name);
                                    
                                    
                                    
                                    CREATE TABLE budget_items
                                    (
                                        id            INTEGER PRIMARY KEY,
                                        description   TEXT,
                                        monthly_value NUMERIC DEFAULT 0 CHECK(monthly_value >= 0),
                                        note          TEXT,
                                        category_id   NUMERIC DEFAULT 0,
                                        date_created  TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag          TEXT,
                                        FOREIGN KEY(category_id) REFERENCES categories(id)

                                    );
                                    CREATE INDEX idx__budget_items__description ON budget_items(description);
                                    CREATE INDEX idx__budget_items__category_id ON categories(id);
                                    
                                    
                                    
                                    CREATE TABLE expense_items
                                    (
                                        id            INTEGER PRIMARY KEY,
                                        description   TEXT,
                                        monthly_value NUMERIC DEFAULT 0 CHECK(monthly_value < 0),
                                        note          TEXT,
                                        category_id   NUMERIC DEFAULT 0,
                                        date_created  TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag          TEXT,
                                        FOREIGN KEY(category_id) REFERENCES categories(id)
                                    );
                                    CREATE INDEX idx__expense_items__description ON expense_items(description);
                                    CREATE INDEX idx__expense_items__category_id ON categories(id);
                                    
                                    
                                    
                                    CREATE TABLE cashflow_entries
                                    (
                                        id                  INTEGER PRIMARY KEY,
                                        account_id          NUMERIC DEFAULT 0,
                                        date                TEXT,
                                        balance             NUMERIC DEFAULT 0,
                                        note                TEXT,
                                        date_created        TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified       TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag                TEXT,
                                        FOREIGN KEY(account_id) REFERENCES account_profiles(id)
                                    );
                                    CREATE INDEX idx__cashflow_entries__account_id ON account_profiles(id);
                                    
                                    
                                    
                                    CREATE TABLE income_items
                                    (
                                        id                  INTEGER PRIMARY KEY,
                                        name                TEXT NOT NULL,
                                        income_profile_id   NUMERIC DEFAULT 0,
                                        monthly_value       NUMERIC DEFAULT 0,
                                        date_created        TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified       TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag                TEXT,
                                        FOREIGN KEY(income_profile_id) REFERENCES income_profiles(id)
                                    );
                                    CREATE UNIQUE INDEX idx__income_items__income_profile_id ON income_profiles(id);
                                    
                                    
                                    
                                    
                                    CREATE TABLE transactions
                                    (
                                        id                  INTEGER PRIMARY KEY,
                                        description         TEXT,
                                        source              TEXT,
                                        date                TEXT NOT NULL,
                                        value               NUMERIC DEFAULT 0,
                                        category_id         NUMERIC DEFAULT 0,
                                        note                TEXT,
                                        date_created        TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        date_modified       TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                        flag                TEXT,
                                        FOREIGN KEY(category_id) REFERENCES categories(id)
                                    );
                                    CREATE INDEX idx__transactions__category_id ON categories(id);
                                    
                                    
                                    """;


        public static async Task Migrate(IOptions<DatabaseSettings> databaseSettings)
        {
            // Create connection string.
            var builder = new SqliteConnectionStringBuilder()
            {
                ForeignKeys = true,
                DataSource = databaseSettings.Value.Path
            };

            // Connect to the backend.
            var connection = new SqliteConnection(builder.ConnectionString);
            connection.Open();

            // Initiate a transaction. Pass to every ExecuteAsync method. 
            await using var transaction = await connection.BeginTransactionAsync();

            // Grab current schema value in database.
            //TODO: Investigate - This query yields -1 in the current database, not zero?

            // Note:  Manually settings the value inside DB Browser has no effect on this.
            var version = await connection.ExecuteAsync($"PRAGMA user_version;", transaction: transaction);

            // Throw exception if DB's value is past migration version.
            if (version > CurrentVersion)
                throw new InvalidDataException("Database schema version exceeds current version.");

            // Apply migrations until version matches.
            while (version < CurrentVersion)
            {
                ++version;
                await ApplyMigration(connection, version, transaction: transaction);
                //update version:
                await connection.ExecuteAsync($"PRAGMA user_version = {version};", transaction: transaction);
            }

            // Update user version to match current version.
            await transaction.CommitAsync();
        }

        private static async Task ApplyMigration(IDbConnection connection, int version, IDbTransaction transaction)
        {
            var sql = version switch
            {
                1 => Version1,
                _ => throw new ArgumentOutOfRangeException("Failed to apply migration. Version unknown.")
            };
            await connection.ExecuteAsync(sql, transaction: transaction);

        }




    }
}
