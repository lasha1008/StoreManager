using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Facade.Interfaces.Services;

namespace StoreManager.Services;

public sealed class EmployeeQueryService : QueryServiceBase<Employee, IEmployeeRepository>, IEmployeeQueryService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeQueryService(IUnitOfWork unitOfWork) : base(unitOfWork.EmployeeRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public IEnumerable<Employee> Search(string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));

        return _unitOfWork
            .EmployeeRepository
            .Set()
            .Where(x => x.FirstName.Contains(text) ||
                        x.LastName.Contains(text) ||
                        (x.Phone != null && x.Phone.Contains(text)) ||
                        (x.Email != null && x.Email.Contains(text))
            );
    }
}
