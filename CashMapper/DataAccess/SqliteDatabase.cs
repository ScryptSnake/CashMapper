using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;

namespace CashMapper.DataAccess
{
    public class SqliteDatabase: IDatabase
    {
        private IDbConnection Connection { get; }

        public SqliteDatabase(DatabaseSettings dbSettings)
        {
            var builder = new SqliteConnectionStringBuilder()
            {
                ForeignKeys = true,
                DataSource = dbSettings.Path
            };
            // Connect to new db instance.
            // Store the connection for use with dapper. 
            Connection = new SqliteConnection(builder.ConnectionString);
            Connection.Open();
            
        }

        public ConnectionState State
        {
            get => Connection.State;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteAsync(string sql, object? param = null)
        {
            await Connection.ExecuteAsync(sql, param);
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
           return await Connection.ExecuteScalarAsync<T>(sql, param);
        }

        public DbTransaction BeginTransaction()
        {
            return new DbTransaction(Connection.BeginTransaction());
        }

        public async Task<TEntity> GetAsync<TEntity>(string sql, object? param = null)
        {
            return await Connection.QuerySingleAsync<TEntity>(sql, param);
        }

        public async Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity>(string sql, object? param = null)
        {
            return await Connection.QueryAsync<TEntity>(sql, param);
        }

    }
}
