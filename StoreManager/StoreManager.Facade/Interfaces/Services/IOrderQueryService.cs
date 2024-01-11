using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface IOrderQueryService : IQueryService<Order>
{
    IEnumerable<Order> Set(DateTime fromDate, DateTime toDate);
    IEnumerable<Order> Search(string text);
}
