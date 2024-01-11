using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public class ProductRepositoryTests : RepositoryUnitTestBase
{
    public ProductRepositoryTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void Insert(string name, decimal price)
    {
        Product product = GetTestRecord(name, price);
        _unitOfWork.ProductRepository.Insert(product);
        _unitOfWork.SaveChanges();

        Assert.True(product.Id > 0);
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void NotInserted(string name, decimal price)
    {
        Product product = GetTestRecord(name, price);
        product.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void Update(string name, decimal price)
    {
        Product newProduct = GetTestRecord(name, price);
        _unitOfWork.ProductRepository.Insert(newProduct);
        _unitOfWork.SaveChanges();

        newProduct.Name = $"New {name}";
        newProduct.Price = price;
        _unitOfWork.ProductRepository.Update(newProduct);
        _unitOfWork.SaveChanges();

        Product updatedProduct = _unitOfWork.ProductRepository.Set(x => x.Id == newProduct.Id).Single();

        Assert.True(updatedProduct.Name == newProduct.Name && updatedProduct.Price == newProduct.Price);
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void NotUpdated(string name, decimal price)
    {
        Product newProduct = GetTestRecord(name, price);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            _unitOfWork.ProductRepository.Update(newProduct);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void DeleteByObject(string name, decimal price)
    {
        Product product = GetTestRecord(name, price);
        _unitOfWork.ProductRepository.Insert(product);
        _unitOfWork.SaveChanges();

        _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.ProductRepository.Set(x => x.Id == product.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void DeleteById(string name, decimal price)
    {
        Product product = GetTestRecord(name, price);
        _unitOfWork.ProductRepository.Insert(product);
        _unitOfWork.SaveChanges();

        _unitOfWork.ProductRepository.Delete(product.Id);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.ProductRepository.Set(x => x.Id == product.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void GetById(string name, decimal price)
    {
        Product newProduct = GetTestRecord(name, price);
        _unitOfWork.ProductRepository.Insert(newProduct);
        _unitOfWork.SaveChanges();

        Product retrievedProduct = _unitOfWork.ProductRepository.Get(newProduct.Id);

        Assert.True(retrievedProduct.Id == newProduct.Id);
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void Set(string name, decimal price)
    {
        Product newProduct = GetTestRecord(name, price);
        List<Product> expectedSet = new();
        expectedSet.Add(newProduct);

        _unitOfWork.ProductRepository.Insert(newProduct);
        _unitOfWork.SaveChanges();

        var retrievedProducts = _unitOfWork.ProductRepository.Set();

        Assert.True(expectedSet.Last().Name == retrievedProducts.Last().Name &&
                    expectedSet.Last().Price == retrievedProducts.Last().Price
            );
    }

    private static Product GetTestRecord(string name, decimal price)
    {
        Product product = new()
        {
            Name = name,
            Price = price
        };
        return product;
    }
}