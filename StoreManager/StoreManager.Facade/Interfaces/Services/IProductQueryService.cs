using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface IProductQueryService : IQueryService<Product>
{
    IEnumerable<Product> Search(string text);
}
