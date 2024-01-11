using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class CountryCommandService : CommandServiceBase<Country, ICountryRepository>, ICountryCommandService
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryCommandService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.CountryRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}
