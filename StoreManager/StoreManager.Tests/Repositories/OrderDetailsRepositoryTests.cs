using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public class OrderDetailsRepositoryTests : RepositoryUnitTestBase
{
    public OrderDetailsRepositoryTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData(1, 1, 1.3, 10)]
    [InlineData(2, 2, 3.4, 44)]
    [InlineData(3, 3, 7.2, 50)]
    [InlineData(4, 4, 2.99, 2)]
    public void Insert(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails orderDetails = GetTestRecord(orderId, productId, price, quantity);
        _unitOfWork.OrderDetailsRepository.Insert(orderDetails);
        _unitOfWork.SaveChanges();

        var retrievedOrderDetails = _unitOfWork.OrderDetailsRepository.Set().Where(x => x.OrderId == orderId && x.ProductId == productId);
        Assert.NotNull(retrievedOrderDetails);
    }

    [Theory]
    [InlineData(1, 1, 1.3, 10)]
    [InlineData(1, 1, 3.4, 44)]
    [InlineData(1, 1, 7.2, 50)]
    [InlineData(1, 1, 2.99, 2)]
    public void NotInserted(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails orderDetails = GetTestRecord(orderId, productId, price, quantity);

        Assert.Throws<ArgumentException>(() =>
        {
            _unitOfWork.OrderDetailsRepository.Insert(orderDetails);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData(5, 5, 1.3, 10)]
    [InlineData(6, 6, 3.4, 44)]
    [InlineData(7, 7, 7.2, 50)]
    [InlineData(8, 8, 2.99, 2)]
    public void Update(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails newOrderDetails = GetTestRecord(orderId, productId, price, quantity);
        _unitOfWork.OrderDetailsRepository.Insert(newOrderDetails);
        _unitOfWork.SaveChanges();

        newOrderDetails.Price = quantity;
        newOrderDetails.Quantity = (int)price;
        _unitOfWork.OrderDetailsRepository.Update(newOrderDetails);
        _unitOfWork.SaveChanges();

        OrderDetails updatedOrderDetails = _unitOfWork.OrderDetailsRepository
                                .Set(x => x.OrderId == newOrderDetails.OrderId && x.ProductId == newOrderDetails.ProductId)
                                .Single();

        Assert.NotNull(updatedOrderDetails);
        Assert.True(updatedOrderDetails.Price == newOrderDetails.Price && updatedOrderDetails.Quantity == newOrderDetails.Quantity);
    }

    [Theory]
    [InlineData(9, 9, 1.3, 10)]
    [InlineData(10, 10, 3.4, 44)]
    [InlineData(11, 11, 7.2, 50)]
    [InlineData(12, 12, 2.99, 2)]
    public void NotUpdated(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails newOrderDetails = GetTestRecord(orderId, productId, price, quantity);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            _unitOfWork.OrderDetailsRepository.Update(newOrderDetails);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData(13, 13, 1.3, 10)]
    [InlineData(14, 14, 3.4, 44)]
    [InlineData(15, 15, 7.2, 50)]
    [InlineData(16, 16, 2.99, 2)]
    public void DeleteByObject(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails orderDetails = GetTestRecord(orderId, productId, price, quantity);
        _unitOfWork.OrderDetailsRepository.Insert(orderDetails);
        _unitOfWork.SaveChanges();

        _unitOfWork.OrderDetailsRepository.Delete(orderDetails);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.OrderDetailsRepository.Set(x => x.OrderId == orderDetails.OrderId && x.ProductId == orderDetails.ProductId).SingleOrDefault());
    }

    [Theory]
    [InlineData(17, 17, 1.3, 10)]
    [InlineData(18, 18, 3.4, 44)]
    [InlineData(19, 19, 7.2, 50)]
    [InlineData(20, 20, 2.99, 2)]
    public void DeleteById(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails orderDetails = GetTestRecord(orderId, productId, price, quantity);
        _unitOfWork.OrderDetailsRepository.Insert(orderDetails);
        _unitOfWork.SaveChanges();

        _unitOfWork.OrderDetailsRepository.Delete(orderDetails.OrderId, orderDetails.ProductId);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.OrderDetailsRepository.Set(x => x.OrderId == orderDetails.OrderId && x.ProductId == orderDetails.ProductId).SingleOrDefault());
    }

    [Theory]
    [InlineData(21, 21, 1.3, 10)]
    [InlineData(22, 22, 3.4, 44)]
    [InlineData(23, 23, 7.2, 50)]
    [InlineData(24, 24, 2.99, 2)]
    public void GetById(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails newOrderDetails = GetTestRecord(orderId, productId, price, quantity);
        _unitOfWork.OrderDetailsRepository.Insert(newOrderDetails);
        _unitOfWork.SaveChanges();

        OrderDetails retrievedOrderDetails = _unitOfWork.OrderDetailsRepository.Get(newOrderDetails.OrderId, newOrderDetails.ProductId);

        Assert.True(retrievedOrderDetails.ProductId == newOrderDetails.ProductId && retrievedOrderDetails.OrderId == newOrderDetails.OrderId);
    }

    [Theory]
    [InlineData(25, 25, 1.3, 10)]
    [InlineData(26, 26, 3.4, 44)]
    [InlineData(27, 27, 7.2, 50)]
    [InlineData(28, 28, 2.99, 2)]
    public void Set(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails newOrderDetails = GetTestRecord(orderId, productId, price, quantity);
        List<OrderDetails> expectedSet = new();
        expectedSet.Add(newOrderDetails);

        _unitOfWork.OrderDetailsRepository.Insert(newOrderDetails);
        _unitOfWork.SaveChanges();

        var retrievedOrderDetails = _unitOfWork.OrderDetailsRepository.Set();

        Assert.True(expectedSet.Last().Price == retrievedOrderDetails.Last().Price &&
                    expectedSet.Last().Quantity == retrievedOrderDetails.Last().Quantity
                );
    }

    private static OrderDetails GetTestRecord(int orderId, int productId, decimal price, int quantity)
    {
        OrderDetails orderDetails = new()
        {
            OrderId = orderId,
            ProductId = productId,
            Price = price,
            Quantity = quantity
        };
        return orderDetails;
    }
}
