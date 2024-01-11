using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Repositories;

internal sealed class CountryRepository : RepositoryBase<Country>, ICountryRepository
{
    public CountryRepository(StoreManagerDbContext context) : base(context) { }
}