using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;


namespace CashMapperWebApi.Controllers;

[Route("api/AccountProfiles")]
[ApiController]
public class AccountProfilesController : ControllerBase, ICashMapperModelController<AccountProfile>
{
    private AccountProfileRepository Repository { get; }
    private ILogger<AccountProfile> Logger { get; }
    public AccountProfilesController(IRepository<AccountProfile> repository, ILogger<AccountProfile> logger)
    {
        Repository = (AccountProfileRepository)repository;
        Logger = logger;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<AccountProfile>> GetAsync(long id)
    {
        Logger.LogDebug("GET request made for ID={id}", id);
        var result = await Repository.FindAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
        
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<AccountProfile>> GetByNameAsync(string name)
    {
        Logger.LogDebug("GET request made for Name={name}", name);
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
        Logger.LogDebug("GET request made for all data.");
        var result = await Repository.GetAllAsync();
        return Ok(result);  
    }

    [HttpPost]
    public async Task<ActionResult<AccountProfile>> AddItemAsync([FromBody] AccountProfile model)
    {
        Logger.LogDebug("POST request made with data: {model}.", model.ToString());
        var result = await Repository.AddAsync(model);
       // Return a 201 Created response with the location of the created resource
       // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
       // Omit the suffix and explicitly provide.
       return CreatedAtAction("Get", new {id=result.Id}, result);
    }

    [HttpPut]
    public async Task<ActionResult<AccountProfile>> UpdateItemAsync(AccountProfile model)
    {
        Logger.LogDebug("PUT request made with data: {model}",model);
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }
}
