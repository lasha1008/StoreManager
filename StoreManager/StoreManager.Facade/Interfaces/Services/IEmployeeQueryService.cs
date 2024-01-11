using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface IEmployeeQueryService : IQueryService<Employee>
{
    IEnumerable<Employee> Search(string text);
}
