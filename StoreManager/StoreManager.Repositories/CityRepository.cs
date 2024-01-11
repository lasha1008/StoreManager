using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Repositories;

internal sealed class CityRepository : RepositoryBase<City>, ICityRepository
{
    public CityRepository(StoreManagerDbContext context) : base(context) { }
}