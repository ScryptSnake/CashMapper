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
        private IRepository<Category> Repository { get; }

        public CategoriesController(IRepository<Category> repository)
        {
            Repository = repository;
        }



        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await Repository.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(long id)
        {
            var result = await Repository.FindAsync(id);
            return result.ToString();
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpGet]
        public Task<Category> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<IEnumerable<Category>> GetAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("{model}")]
        public Task<ActionResult<Category>> PostAsync(Category model)
        {
            throw new NotImplementedException();

            return new CreatedAtActionResult()

        }

        [HttpPut("{model}")]
        public Task<ActionResult<Category>> PutAsync(Category model)
        {
            throw new NotImplementedException();
        }
    }
}
