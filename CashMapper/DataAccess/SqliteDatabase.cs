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




        public void Dispose()
        {
            Connection.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            Connection.DisposeAsync();
        }


        public Task ExecuteAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync<TParam>(string query, TParam? parameters)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync<TEntity>(string query)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync<TEntity, TParam>(string query, TParam? parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity>(string query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity, TParam>(string query, TParam? parameters)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync<TEntity>(TEntity entity, string query)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync<TEntity, TParam>(TEntity entity, string query, TParam? parameters)
        {
            throw new NotImplementedException();
        }
    }
}
