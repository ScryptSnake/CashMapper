using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace CashMapperWebApi.Controllers;

[Route("api/AccountProfiles")]
[ApiController]
public class AccountProfilesController : ControllerBase, ICashMapperModelController<AccountProfile>
{
    private AccountProfileRepository Repository { get; }
    public AccountProfilesController(IRepository<AccountProfile> repository)
    {
        Repository = (AccountProfileRepository)repository;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<AccountProfile>> GetAsync(long id)
    {
        var result = await Repository.FindAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<AccountProfile>> GetByNameAsync(string name)
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
    public async Task<ActionResult<IEnumerable<AccountProfile>>> GetAllAsync()
    {
        var result = await Repository.GetAllAsync();
        return Ok(result);  
    }

    [HttpPost]
    public async Task<ActionResult<AccountProfile>> AddItemAsync([FromBody] AccountProfile model)
    {
       var result = await Repository.AddAsync(model);
       // Return a 201 Created response with the location of the created resource
       // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
       // Omit the suffix and explicitly provide.
       return CreatedAtAction("Get", new {id=result.Id}, result);
    }

    [HttpPut]
    public async Task<ActionResult<AccountProfile>> UpdateItemAsync(AccountProfile model)
    {
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }
}
