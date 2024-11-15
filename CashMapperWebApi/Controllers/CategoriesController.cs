using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CashMapperWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase, ICashMapperModelController<Category>
    {
        private CategoryRepository Repository { get; }

        public CategoriesController(IRepository<Category> repository)
        {
            Repository = (CategoryRepository)repository;
        }

        [HttpGet("{id:long}")]
        public async Task<Category> GetAsync(long id)
        {
            return await Repository.FindAsync(id);
        }

        [HttpGet("{name}")]
        public async Task<Category> GetAsync(string name)
        {
            return await Repository.GetByName(name);    
        }



        [HttpGet]
        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await Repository.GetAllAsync();
        }

        [HttpPost("{model}")]
        public async Task<ActionResult<Category>> PostAsync(Category model)
        {
           var result = await Repository.AddAsync(model);
           return new ActionResult<Category>(result);

        }

        [HttpPut("{model}")]
        public async Task<ActionResult<Category>> PutAsync(Category model)
        {
            var result = await Repository.UpdateAsync(model);
            return new ActionResult<Category>(result);
        }
    }
}
