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
    public TransactionsController(IRepository<Transaction> repository, IRepository<Category> categoryRepository)
    {
        Repository = (TransactionRepository)repository;
        CategoryRepository = (CategoryRepository)categoryRepository;   
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Transaction>> GetAsync(long id)
    {
        var result = await Repository.FindAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync([FromQuery] string? description, 
        [FromQuery] string? categoryName, [FromQuery] string? source, [FromQuery] string? flag,
        [FromQuery] decimal? minValue, [FromQuery] decimal? maxValue, 
        [FromQuery] DateTimeOffset? minDate, [FromQuery] DateTimeOffset? maxDate)
    {
        // Find category Id if provided (filter only accepts an id, not by name).
        long? categoryId = null;
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
        var result = await Repository.GetAllAsync();
        return Ok(result);  
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> AddItemAsync([FromBody] Transaction model)
    {
       var result = await Repository.AddAsync(model);
       // Return a 201 Created response with the location of the created resource
       // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
       // Omit the suffix and explicitly provide.
       return CreatedAtAction("Get", new {id=result.Id}, result);
    }

    [HttpPut]
    public async Task<ActionResult<Transaction>> UpdateItemAsync(Transaction model)
    {
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }
}
