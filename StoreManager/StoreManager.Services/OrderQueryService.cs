using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public class OrderQueryService : QueryServiceBase<Order, IOrderRepository>, IOrderQueryService
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderQueryService(IUnitOfWork unitOfWork) : base(unitOfWork.OrderRepository)
    {
        _unitOfWork = unitOfWork?? throw new ArgumentException(nameof(unitOfWork));
    }

    public IEnumerable<Order> Set(DateTime fromDate, DateTime toDate) =>
        Set(x => x.OrderDate >= fromDate && x.OrderDate <= toDate);

    public IEnumerable<Order> Search(string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        var dateTime = DateTime.Parse(text);

        return _unitOfWork
            .OrderRepository
            .Set()
            .Where(x => x.OrderDate.Equals(dateTime) || x.ShippedDate.Equals(dateTime) || x.ShipAddress != null && x.ShipAddress.Contains(text));
    }
}
