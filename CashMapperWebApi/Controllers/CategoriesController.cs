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
        public async Task<ActionResult<Category>> GetAsync(long id)
        {
            var exists = await Repository.ExistsAsync(new Category(){Id=id});
            if (!exists)
            {
                return NotFound();
            }
            var result = await Repository.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Category>> GetAsync(string name)
        {
            var exists = await Repository.ExistsByNameAsync(name);
            if (!exists)
            {
                return NotFound();
            }
            var result = await Repository.GetByNameAsync(name);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAsync()
        {
            var result = await Repository.GetAllAsync();
            return Ok(result);  
        }

        [HttpPost("{model}")]
        public async Task<ActionResult<Category>> PostAsync([FromBody] Category model)
        {
           var result = await Repository.AddAsync(model);
           // Return a 201 Created response with the location of the created resource
           var act = nameof(GetAsync);

           return CreatedAtAction(act, new {id=result.Id}, result);


        }

        [HttpPut("{model}")]
        public async Task<ActionResult<Category>> PutAsync(Category model)
        {
            var result = await Repository.UpdateAsync(model);
            return Ok(result);
        }
    }
}
