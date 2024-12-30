using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Filters;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CashMapperWebApi.Controllers;

[Route("api/Transactions")]
[ApiController]
public class TransactionsController : ControllerBase, ICashMapperModelController<Transaction>
{
    private TransactionRepository Repository { get; }
    private CategoryRepository CategoryRepository { get; }

    private ILogger<Transaction> Logger { get; }
    public TransactionsController(IRepository<Transaction> repository, IRepository<Category> categoryRepository, ILogger<Transaction> logger)
    {
        Repository = (TransactionRepository)repository;
        CategoryRepository = (CategoryRepository)categoryRepository;
        Logger = logger;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Transaction>> GetAsync(long id)
    {
        Logger.LogInformation("GET request made for ID={id}", id);

        var result = await Repository.FindAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }


    // Simplified version. More ambiguous for how params should be provided. 
    [HttpGet("filter-by-object")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync([FromQuery] TransactionQueryFilter filter)
    {

        Logger.LogInformation("GET request made with filter: {filter}",filter.ToString());

        // Query the backend with the filter.
        var results = await Repository.Query(filter);
        if (!results.Any())
        {
            return NoContent();
        }
        return Ok(results);
    }



    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync(
        [FromQuery] string? description,
        [FromQuery] string? categoryName, [FromQuery] string? source, [FromQuery] string? flag,
        [FromQuery] decimal? minValue, [FromQuery] decimal? maxValue,
        [FromQuery] DateTimeOffset? minDate, [FromQuery] DateTimeOffset? maxDate)
    {
        // Find category Id if provided (filter only accepts an id, not by name).
        long ? categoryId = null;
        if (categoryName != null)
        {
            var category = await CategoryRepository.GetByNameAsync(categoryName);
            categoryId = category.Id;
        }

        // Assemble a filter object from http query params.
        var filter = new TransactionQueryFilter()
        {
            DescriptionLike = description,
            CategoryId = categoryId,
            Source = source,
            Flag = flag,
            ValueRange = (minValue, maxValue),
            DateRange = (minDate, maxDate)
        };

        Logger.LogInformation("GET request made for filter: {filter}", filter.ToString());

        // Query the backend with the filter.
        var results = await Repository.Query(filter);
        if (!results.Any())
        {
            return NoContent();
        }
        return Ok(results);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllAsync()
    {
        Logger.LogInformation("GET request made for all data.");
        var result = await Repository.GetAllAsync();
        return Ok(result);  
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> AddItemAsync([FromBody] Transaction model)
    {
        Logger.LogInformation("POST request made with data: {model}", model.ToString());
        var result = await Repository.AddAsync(model);
        // Return a 201 Created response with the location of the created resource
        // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
        // Omit the suffix and explicitly provide.
        return CreatedAtAction("Get", new {id=result.Id}, result);
    }

    [HttpPut]
    public async Task<ActionResult<Transaction>> UpdateItemAsync(Transaction model)
    {
        Logger.LogInformation("PUT request made with data: {model}", model.ToString());
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }
}
