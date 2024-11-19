using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CashMapperWebApi.Controllers;

[Route("api/Categories")]
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
        var result = await Repository.FindAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Category>> GetByNameAsync(string name)
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
    public async Task<ActionResult<IEnumerable<Category>>> GetAllAsync()
    {
        var result = await Repository.GetAllAsync();
        return Ok(result);  
    }

    [HttpPost]
    public async Task<ActionResult<Category>> AddItemAsync([FromBody] Category model)
    {
       var result = await Repository.AddAsync(model);
       // Return a 201 Created response with the location of the created resource
       // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
       // Omit the suffix and explicitly provide.
       return CreatedAtAction("Get", new {id=result.Id}, result);
    }

    [HttpPut]
    public async Task<ActionResult<Category>> UpdateItemAsync(Category model)
    {
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }
}
