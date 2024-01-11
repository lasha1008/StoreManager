using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Query;

public class CategoryQueryServiceTests : QueryUnitTestsBase
{
    public CategoryQueryServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Get(string name, string description)
    {
        Category newCategory = GetTestRecord(name, description);
        _unitOfWork.CategoryRepository.Insert(newCategory);
        _unitOfWork.SaveChanges();

        CategoryQueryService categoryQueryService = new(_unitOfWork);

        Category retrievedCategory = categoryQueryService.Get(newCategory.Id);

        Assert.True(retrievedCategory.Id == newCategory.Id);
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Set(string name, string description)
    {
        CategoryQueryService categoryQueryService = new(_unitOfWork);

        Category newCategory = GetTestRecord(name, description);
        List<Category> expectedSet = new();
        expectedSet.Add(newCategory);

        _unitOfWork.CategoryRepository.Insert(newCategory);
        _unitOfWork.SaveChanges();

        var retrievedCategories = categoryQueryService.Set();

        Assert.True(expectedSet.Last().Name == retrievedCategories.Last().Name &&
                    expectedSet.Last().Description == retrievedCategories.Last().Description
                );
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void ExpressionSet(string name, string description)
    {
        CategoryQueryService categoryQueryService = new(_unitOfWork);

        Category newCategory = GetTestRecord(name, description);
        List<Category> expectedSet = new();
        expectedSet.Add(newCategory);

        _unitOfWork.CategoryRepository.Insert(newCategory);
        _unitOfWork.SaveChanges();

        var retrievedCategories = categoryQueryService.Set(x => x.Name == name);

        Assert.True(expectedSet.Last().Name == retrievedCategories.Last().Name &&
                    expectedSet.Last().Description == retrievedCategories.Last().Description
                );
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
