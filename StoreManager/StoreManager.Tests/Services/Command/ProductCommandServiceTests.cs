using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Command;

public class ProductCommandServiceTests : CommandUnitTestsBase
{
    public ProductCommandServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)] 
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void Insert(string name, decimal price)
    {
        Product product = GetTestRecord(name ,price);
        ProductCommandService productCommandService = new(_unitOfWork);
        productCommandService.Insert(product);

        Assert.NotNull(product.Name);
        Assert.True(product.Id > 0);
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void NotInserted(string name,decimal price)
    {
        Product product = GetTestRecord(name,  price);
       product.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            ProductCommandService productCommandService = new(_unitOfWork);
            productCommandService.Insert(product);
        });
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void Update(string name, decimal price)
    {
        ProductCommandService productCommandService = new(_unitOfWork);

        Product newProduct = GetTestRecord(name, price);
        productCommandService.Insert(newProduct);

        newProduct.Name = $"New {name}";
        newProduct.Price = price;

        productCommandService.Update(newProduct);

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
            ProductCommandService productCommandService = new(_unitOfWork);
           productCommandService.Update(newProduct);
        });
    }

    [Theory]
    [InlineData("Product 1", 1)]
    [InlineData("Product 2", 2)]
    [InlineData("Product 3", 3)]
    [InlineData("Product 4", 4)]
    public void Delete(string name, decimal price)
    {
       ProductCommandService productCommandService = new(_unitOfWork);

       Product product = GetTestRecord(name, price);
       productCommandService.Insert(product);

       productCommandService.Delete(product);

        Assert.True(_unitOfWork.ProductRepository.Set(x => x.Id == product.Id).Single().IsDeleted);
    }
    private static Product GetTestRecord(string name,decimal price)
    {
        Product product = new()
        {
            Name = name,
            Price = price
        };
        return product;
    }
}
 
