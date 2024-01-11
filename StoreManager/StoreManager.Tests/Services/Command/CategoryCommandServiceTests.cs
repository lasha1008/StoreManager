using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Command;

public class CategoryCommandServiceTests : CommandUnitTestsBase
{
    public CategoryCommandServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Insert(string name, string description)
    {
        Category category = GetTestRecord(name, description);
        CategoryCommandService categoryCommandService = new(_unitOfWork);
        categoryCommandService.Insert(category);

        Assert.NotNull(category.Name);
        Assert.True(category.Id > 0);
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void NotInserted(string name, string description)
    {
        Category category = GetTestRecord(name, description);
        category.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            CategoryCommandService categoryCommandService = new(_unitOfWork);
            categoryCommandService.Insert(category);
        });
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Update(string name, string description)
    {
        CategoryCommandService categoryCommandService = new(_unitOfWork);

        Category newCategory = GetTestRecord(name, description);
        categoryCommandService.Insert(newCategory);

        newCategory.Name = $"New {name}";
        newCategory.Description = $"New test {description}.";

        categoryCommandService.Update(newCategory);

        Category updatedCategory = _unitOfWork.CategoryRepository.Set(x => x.Id == newCategory.Id).Single();

        Assert.True(updatedCategory.Name == newCategory.Name && updatedCategory.Description == newCategory.Description);
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void NotUpdated(string name, string description)
    {
        Category newCategory = GetTestRecord(name, description);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            CategoryCommandService categoryCommandService = new(_unitOfWork);
            categoryCommandService.Update(newCategory);
        });
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Delete(string name, string description)
    {
        CategoryCommandService categoryCommandService = new(_unitOfWork);

        Category category = GetTestRecord(name, description);
        categoryCommandService.Insert(category);

        categoryCommandService.Delete(category);

        Assert.True(_unitOfWork.CategoryRepository.Set(x => x.Id == category.Id).Single().IsDeleted);
    }

    private static Category GetTestRecord(string name, string description)
    {
        Category category = new()
        {
            Name = name,
            Description = description
        };
        return category;
    }
}
