using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    void DeleteAccount(Customer customer);
}