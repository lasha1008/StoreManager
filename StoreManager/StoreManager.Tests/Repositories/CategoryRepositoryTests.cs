using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public class CategoryRepositoryTests : RepositoryUnitTestBase
{
    public CategoryRepositoryTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Insert(string name, string description)
    {
        Category category = GetTestRecord(name, description);
        _unitOfWork.CategoryRepository.Insert(category);
        _unitOfWork.SaveChanges();

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
            _unitOfWork.CategoryRepository.Insert(category);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Update(string name, string description)
    {
        Category newCategory = GetTestRecord(name, description);
        _unitOfWork.CategoryRepository.Insert(newCategory);
        _unitOfWork.SaveChanges();

        newCategory.Name = $"New {name}";
        newCategory.Description = $"New test {description}.";
        _unitOfWork.CategoryRepository.Update(newCategory);
        _unitOfWork.SaveChanges();

        Category updatedCategory = _unitOfWork.CategoryRepository.Set(x => x.Id == newCategory.Id).Single();

        Assert.NotNull(updatedCategory);
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
            _unitOfWork.CategoryRepository.Update(newCategory);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void DeleteByObject(string name, string description)
    {
        Category category = GetTestRecord(name, description);
        _unitOfWork.CategoryRepository.Insert(category);
        _unitOfWork.SaveChanges();

        _unitOfWork.CategoryRepository.Delete(category);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.CategoryRepository.Set(x => x.Id == category.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void DeleteById(string name, string description)
    {
        Category category = GetTestRecord(name, description);
        _unitOfWork.CategoryRepository.Insert(category);
        _unitOfWork.SaveChanges();

        _unitOfWork.CategoryRepository.Delete(category.Id);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.CategoryRepository.Set(x => x.Id == category.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void GetById(string name, string description)
    {
        Category newCategory = GetTestRecord(name, description);
        _unitOfWork.CategoryRepository.Insert(newCategory);
        _unitOfWork.SaveChanges();

        Category retrievedCategory = _unitOfWork.CategoryRepository.Get(newCategory.Id);

        Assert.True(retrievedCategory.Id == newCategory.Id);
    }

    [Theory]
    [InlineData("Category 1", "Description 1")]
    [InlineData("Category 2", "Description 2")]
    [InlineData("Category 3", "Description 3")]
    [InlineData("Category 4", "Description 4")]
    public void Set(string name, string description)
    {
        Category newCategory = GetTestRecord(name, description);
        List<Category> expectedSet = new();
        expectedSet.Add(newCategory);

        _unitOfWork.CategoryRepository.Insert(newCategory);
        _unitOfWork.SaveChanges();

        var retrievedCategories = _unitOfWork.CategoryRepository.Set();

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