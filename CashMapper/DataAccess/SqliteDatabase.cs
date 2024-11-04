using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace CashMapper.DataAccess
{
    public class SqliteDatabase: IDatabase
    {
        private readonly SqliteConnection connection;

        public SqliteDatabase(DatabaseSettings dbSettings)
        {
            var builder = new SqliteConnectionStringBuilder()
            {
                ForeignKeys = true,
                DataSource = dbSettings.Path
            };
            // Connect to new db instance.
            connection = new SqliteConnection(builder.ConnectionString);
            connection.Open();
        }



        public void Perform(Action<IDbConnection> action)
        {
            action(connection);
        }

        public void Perform(Action<IDbConnection, IDbTransaction> action)
        {
            using var transaction = connection.BeginTransaction();
            action(connection, transaction);
            transaction.Commit();
        }

        public void Perform<TArg>(Action<IDbConnection, TArg> func, TArg argument)
        {
            func(connection, argument);
        }

        public void Perform<TArg>(Action<IDbConnection, TArg, IDbTransaction> func, TArg argument)
        {
            using var transaction = connection.BeginTransaction();
            func(connection, argument, transaction);
            transaction.Commit();
        }

        public async Task PerformAsync(Func<IDbConnection, Task> func)
        {
            await func(connection);
        }

        public async Task PerformAsync<TArg>(Func<IDbConnection, TArg, Task> func, TArg argument)
        {
            await func(connection, argument);
        }

        public async Task PerformAsync<TArg>(Func<IDbConnection, TArg, IDbTransaction, Task> func, TArg argument)
        {
            await using var transaction = connection.BeginTransaction();
            await func(connection, argument, transaction);
            transaction.Commit();
        }

        public TResult Get<TResult>(Func<IDbConnection, TResult> func)
        {
            return func(connection); // Remember:  the last argument in the Func delegate is the return type of the func.
        }

        public TResult Get<TResult>(Func<IDbConnection, IDbTransaction, TResult> func)
        {
            using var transaction = connection.BeginTransaction();
            var result = func(connection, transaction);
            transaction.Commit();
            return result;
        }

        public TResult Get<TResult, TArg>(Func<IDbConnection, TArg, TResult> func, TArg argument)
        {
            throw new NotImplementedException();
        }

        public TResult Get<TResult, TArg>(Func<IDbConnection, TArg, IDbTransaction, TResult> func, TArg argument)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAsync<TResult>(Func<IDbConnection, Task<TResult>> func)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAsync<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> func)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAsync<TResult, TArg>(Func<IDbConnection, TArg, Task<TResult>> func, TArg argument)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAsync<TResult, TArg>(Func<IDbConnection, TArg, IDbTransaction, Task<TResult>> func, TArg argument)
        {
            throw new NotImplementedException();
        }




        public void Dispose()
        {
            connection.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            connection.DisposeAsync();
        }
    }
}
