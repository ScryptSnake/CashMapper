using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.TypeHandlers;
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

            ////Register type mappers for dapper
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());

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

        public async Task<TEntity?> GetAsync<TEntity>(string sql, object? param = null, TEntity? defaultValue=default)
        {
            // Does not throw exception if dapper returns nothing.
            var result = await Connection.QuerySingleOrDefaultAsync<TEntity>(sql, param);
            if (result == null) return defaultValue;
            return result;
        }

        public async Task<TEntity> GetSingleAsync<TEntity>(string sql, object? param = null)
        {
            // Expects 1 record. Throws exception if no matches.
            return await Connection.QuerySingleAsync<TEntity>(sql, param);
        }

        public async Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity>(string sql, object? param = null)
        {
            // Dapper will return an empty enumerable if no results are returned.
            return await Connection.QueryAsync<TEntity>(sql, param);
        }

    }
}
