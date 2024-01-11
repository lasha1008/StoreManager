using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface ICountryQueryService : IQueryService<Country>
{
    IEnumerable<Country> Search(string text);
}
