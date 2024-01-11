using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public abstract class RepositoryUnitTestBase : UnitTestBase
{
    protected readonly IUnitOfWork _unitOfWork;

    public RepositoryUnitTestBase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
}