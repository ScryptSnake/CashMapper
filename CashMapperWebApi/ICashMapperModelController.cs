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
        Task<ActionResult<TModel>> GetAsync(long id);

        // HTTP GET.
        Task<ActionResult<IEnumerable<TModel>>> GetAllAsync();

        // HTTP POST.
        Task<ActionResult<TModel>> AddItemAsync(TModel model);

        // HTTP PUT.
        Task<ActionResult<TModel>> UpdateItemAsync(TModel model);
    }
}
