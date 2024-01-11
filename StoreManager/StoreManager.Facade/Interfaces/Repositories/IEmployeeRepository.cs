using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    void DeleteAccount(Employee employee);
}