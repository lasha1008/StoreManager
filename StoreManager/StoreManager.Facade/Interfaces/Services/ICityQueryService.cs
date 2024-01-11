using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface ICityQueryService : IQueryService<City>
{
    IEnumerable<City> Search(string text);
}
