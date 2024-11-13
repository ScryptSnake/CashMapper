using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMapper.DataAccess.Entities;

namespace CashMapper.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<bool> ExistsAsync(TEntity entity);
        Task<TEntity> FindAsync(long id);
        Task<TEntity> GetAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
