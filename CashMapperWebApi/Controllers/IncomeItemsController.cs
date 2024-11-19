using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace CashMapperWebApi.Controllers;

[Route("api/IncomeItems")]
[ApiController]
public class IncomeItemsController : ControllerBase, ICashMapperModelController<IncomeItem>
{
    private IncomeItemRepository Repository { get; }
    private IncomeProfileRepository ProfileRepository { get; }
    public IncomeItemsController(IRepository<IncomeItem> repository, IRepository<IncomeProfile> profileRepository)
    {
        Repository = (IncomeItemRepository)repository;
        ProfileRepository = (IncomeProfileRepository)profileRepository;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<IncomeItem>> GetAsync(long id)
    {
        var exists = await Repository.ExistsAsync(id);
        if (!exists) return NotFound();
        var result = await Repository.FindAsync(id);
        return Ok(result);
    }

    [HttpGet("by-profile/{incomeProfileId:long}")]
    public async Task<ActionResult<IEnumerable<IncomeItem>>> GetByIncomeProfileAsync(long incomeProfileId)
    {
        var exists = await ProfileRepository.ExistsAsync(incomeProfileId);
        if (!exists) 
            return NotFound(new {message=$"IncomeProfile with ID not found: {incomeProfileId}"});
        var result = await Repository.GetByIncomeProfileId(incomeProfileId);
        if (!result.Any()) return NoContent();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeItem>>> GetAllAsync()
    {
        var result = await Repository.GetAllAsync();
        return Ok(result);   
    }

    [HttpPost]
    public async Task<ActionResult<IncomeItem>> AddItemAsync([FromBody] IncomeItem model)
    {
        var profileExists = await ProfileRepository.ExistsAsync(model.IncomeProfileId);
        var result = await Repository.AddAsync(model);
        // Return a 201 Created response with the location of the created resource
        // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
        // Omit the suffix and explicitly provide.
        return CreatedAtAction("Get", new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<ActionResult<IncomeItem>> UpdateItemAsync(IncomeItem model)
    {
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }
}
