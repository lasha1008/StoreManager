using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CityCommandService : CommandServiceBase<City, ICityRepository>, ICityCommandService
{
    private readonly IUnitOfWork _unitOfWork;

    public CityCommandService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.CityRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}