using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;


namespace CashMapperWebApi.Controllers;

[Route("api/BudgetItems")]
[ApiController]
public class BudgetItemsController : ControllerBase, ICashMapperModelController<BudgetItem>
{
    private BudgetItemRepository Repository { get; }
    private CategoryRepository CategoryRepository { get; }
    public BudgetItemsController(IRepository<BudgetItem> repository, IRepository<Category> profileRepository)
    {
        Repository = (BudgetItemRepository)repository;
        CategoryRepository = (CategoryRepository)profileRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetItem>>> GetAllAsync()
    {
        var result = await Repository.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BudgetItem>> GetAsync(long id)
    {
        var exists = await Repository.ExistsAsync(id);
        if (!exists) return NotFound();
        var result = await Repository.FindAsync(id);
        return Ok(result);
    }

    [HttpGet("by-category-id/{categoryId:long}")]
    public async Task<ActionResult<IEnumerable<BudgetItem>>> GetByCategoryIdAsync(long categoryId)
    {
        var exists = await CategoryRepository.ExistsAsync(categoryId);
        if (!exists)
            return NotFound(new { message = $"Category with ID not found: {categoryId}" });
        var result = await Repository.GetByCategoryId(categoryId);
        if (!result.Any()) return NoContent();
        return Ok(result);
    }

    [HttpGet("by-category-name/{categoryName}")]
    public async Task<ActionResult<IEnumerable<BudgetItem>>> GetByCategoryNameAsync(string categoryName)
    {
        var result = await Repository.GetByCategoryName(categoryName);

        if (!result.Any()) return NoContent();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BudgetItem>> AddItemAsync([FromBody] BudgetItem model)
    {
        var result = await Repository.AddAsync(model);
        // Note: The suffix 'Async' has a bug in ASP, doesn't allow CreatedAtAction to find the method.
        // Omit the suffix and explicitly provide.
        return CreatedAtAction("Get", new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<ActionResult<BudgetItem>> UpdateItemAsync(BudgetItem model)
    {
        var result = await Repository.UpdateAsync(model);
        return Ok(result);
    }

}
