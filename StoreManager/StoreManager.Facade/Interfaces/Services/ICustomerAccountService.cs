using StoreManager.DTO;

namespace StoreManager.Facade.Interfaces.Services;

public interface ICustomerAccountService : IAccountService
{
    void Register(string username, string password, Customer customer);
}
