using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMapper.DataAccess
{
    public interface IRepository<TEntity>
    {
        Task<bool> ExistsAsync(TEntity entity);
        Task<TEntity> FindAsync(TEntity entity);
        Task<TEntity> GetAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetMultipleAsync<TFilter>(TFilter filter);
        Task<IEnumerable<TEntity>> AddAsync(TEntity entity);
    }
}
