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

        Task ExecuteAsync(string query);

        Task ExecuteAsync<TParam>(string query, TParam? parameters);

        Task<TEntity> GetAsync<TEntity>(string query);
        Task<TEntity> GetAsync<TEntity, TParam>(string query, TParam? parameters);

        Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity>(string query);

        Task<IEnumerable<TEntity>> GetMultipleAsync<TEntity,TParam>(string query, TParam? parameters);

        Task<TEntity> AddAsync<TEntity>(TEntity entity, string query);
        Task<TEntity> AddAsync<TEntity,TParam>(TEntity entity, string query, TParam? parameters);

    }
}
 