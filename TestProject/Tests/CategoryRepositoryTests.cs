using CashMapper.DataAccess.Entities;
using CashMapper.DataAccess.Repositories;
using CashMapper.Enums;
using TestProject.Fixtures;

namespace TestProject.Tests
{
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
        public async void CategoryRepository_AddAsync_ShouldReturnMatch()
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
            // These include:  Id, DateCreated, DateModified.
            Assert.Equal(inputEntity.Name, outputEntity.Name);
            Assert.Equal(inputEntity.CategoryType, outputEntity.CategoryType);
            Assert.Equal(inputEntity.Flag,outputEntity.Flag);
        }
    }
}