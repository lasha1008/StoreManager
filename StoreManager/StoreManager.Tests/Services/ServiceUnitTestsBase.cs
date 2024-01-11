using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Services;

public class ServiceUnitTestsBase : UnitTestBase
{
    protected readonly IUnitOfWork _unitOfWork;

    public ServiceUnitTestsBase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}
