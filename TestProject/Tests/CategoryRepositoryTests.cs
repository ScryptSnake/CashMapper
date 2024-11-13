using CashMapper.DataAccess;
using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using CashMapper.Enums;
using TestProject.Fixtures;

namespace TestProject.Tests;

/// <summary>
/// A test class for the income item repository.
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
        Console.WriteLine("Type=" + outputEntity.CategoryType.ToString());
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
        Assert.Equal(outputEntity.CategoryType, newEntity.CategoryType);
        Assert.Equal(outputEntity.Flag, newEntity.Flag);
        Assert.Equal(outputEntity.Name, newEntity.Name);
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
        var exists = await Repository.ExistsAsync(addEntity);

        // Assert.
        Assert.True(exists);
    }


    [Fact]
    public async void CategoryRepository_GetMultipleAsync_ShouldMatch()
    {
        // Arrange.
        var item1 = new Category()
        {
            Name = "TestMultiple1",
            CategoryType = CategoryTypes.Budget,
            Flag = "Flag!"
        };
        var item2 = new Category()
        {
            Name = "TestMultiple2",
            CategoryType = CategoryTypes.Budget,
            Flag = "Flag!"
        };
        var item3 = new Category()
        {
            Name = "TestMultiple3",
            CategoryType = CategoryTypes.Budget,
            Flag = "Flag!"
        };

        var control = new List<Category>();
        control.Add(item1);
        control.Add(item2);
        control.Add(item3);
         
        var filter = new QueryFilter()
            filter.AddCriteria("category_type");


        // Act.
        var result = await Repository.GetMultipleAsync()

        // Assert.
    }

}
}

