using CashMapper.DataAccess;
using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using CashMapper.Enums;
using TestProject.Fixtures;

namespace TestProject.IntegrationTests.WebApi;

/// <summary>
/// An integration test class for the category repository.
/// </summary>

public class CategoryRepositoryTests : IClassFixture<RepositoryFixture>
{
    private RepositoryFixture Fixture { get; }
    private IRepository<Category> Repository { get; }
    public CategoryRepositoryTests(RepositoryFixture fixture)
    {
        // Set fixture.
        Fixture = fixture;
        Repository = fixture.CategoryRepo;
    }

    [Fact]
    public async void CategoryRepository_AddAsync_ShouldMatch()
    {
        // Arrange.
        var inputEntity = new Category()
        {
            Name = "TestCategory3",
            CategoryType = CategoryTypes.Budget,
            Flag = "FLAG",
        };

        // Act.
        var outputEntity = await Repository.AddAsync(inputEntity);

        // Assert.
        // Note: Some properties cannot be tested. Any database generated fields.
        // We do not know their values at design time. 
        // These include:  Id, DateCreated, DateModified.
        Assert.Equal(inputEntity.Name, outputEntity.Name);
        Assert.Equal(inputEntity.CategoryType, outputEntity.CategoryType);
        Assert.Equal(inputEntity.Flag, outputEntity.Flag);
    }

    [Fact]
    public async Task CategoryRepository_FindAsync_ShouldMatch()
    {
        // Arrange,
        var newEntity = new Category()
        {
            Name = "TestCategory4",
            CategoryType = CategoryTypes.Income,
            Flag = "FLAG",
        };

        // Act.
        var addEntity = await Repository.AddAsync(newEntity);
        var id = addEntity.Id;
        var outputEntity = await Repository.FindAsync(id);

        // Assert.
        Assert.Equal(addEntity.Id, outputEntity.Id);
        Assert.Equal(newEntity.CategoryType, outputEntity.CategoryType);
        Assert.Equal(newEntity.Flag, outputEntity.Flag);
        Assert.Equal(newEntity.Name, outputEntity.Name);
    }

    [Fact]
    public async void CategoryRepository_ExistsAsync_ShouldBeTrue()
    {
        // Arrange.
        var newEntity = new Category()
        {
            Name = "TestCategory5",
            CategoryType = CategoryTypes.Budget,
            Flag = "FLAG",
        };
        // Act.
        var addEntity = await Repository.AddAsync(newEntity);
        var exists = await Repository.ExistsAsync(addEntity.Id);

        // Assert.
        Assert.True(exists);
    }

    [Fact]
    public async void CategoryRepository_UpdateAsync_ShouldMatch()
    {
        // Arrange.
        var newEntity = new Category()
        {
            Name = "TestCategory6",
            CategoryType = CategoryTypes.Undefined,
            Flag = "This is a flag.",
        };
        // Act.
        var addEntity = await Repository.AddAsync(newEntity);
        var updatedEntity = await Repository.UpdateAsync(
            addEntity with
            {
                CategoryType = CategoryTypes.Income,
                Flag = "Hello World"
            });

        // Assert.
        Assert.NotEqual(addEntity.CategoryType, updatedEntity.CategoryType);
        Assert.NotEqual(addEntity.Flag, updatedEntity.Flag);
    }
}

