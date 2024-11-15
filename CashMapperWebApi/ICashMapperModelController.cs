using CashMapper.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CashMapperWebApi
{
    /// <summary>
    /// An interface that confines a controller to a set of common methods for data access.
    /// </summary>
    public interface ICashMapperModelController<TModel> where TModel: EntityBase
    {
        // HTTP GET.
        Task<TModel> GetAsync(long id);

        // HTTP GET.
        Task<IEnumerable<TModel>> GetAsync();

        // HTTP POST.
        Task<ActionResult<TModel>> PostAsync(TModel model);

        // HTTP PUT.
        Task<ActionResult<TModel>> PutAsync(TModel model);
    }
}
