using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Repositories;

internal sealed class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(StoreManagerDbContext context) : base(context) { }

    public void DeleteAccount(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        AccountDetails? accountDetails = _context.AccountDetails.Find(employee.Id);
        _context.AccountDetails.Remove(accountDetails!);
    }
}