using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CityQueryService : QueryServiceBase<City, ICityRepository>, ICityQueryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CityQueryService(IUnitOfWork unitOfWork) : base(unitOfWork.CityRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IEnumerable<City> Search(string text)
    {
        if(text == null) throw new ArgumentNullException(nameof(text));

        return _unitOfWork
            .CityRepository
            .Set()
            .Where(x => x.Name.Contains(text));
    }
}
