using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess
{
    /// <summary>
    /// Defines a contract for a Database to be used with this application.
    /// </summary>
    public interface IDatabase: IDisposable, IAsyncDisposable
    {
        ConnectionState State { get; }

        Task ExecuteAsync(string sql, object? param = null);

        Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null);

        DbTransaction BeginTransaction();
        Task<TEntity?> GetAsync<TEntity>(string sql, object? param=null,TEntity? defaultValue=default);

        Task<TEntity> GetSingleAsync<TEntity>(string sql, object? param = null);

        Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity>(string sql, object? param = null);

    }
}
 