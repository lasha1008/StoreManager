using StoreManager.Models;

namespace StoreManager.Facade.Interfaces.Services
{
    public interface IAccountService
    {
        void Login(string username, string password);

        void UpdatePassword(int id, string oldPassword, string newPassword);

        void Unregister(int customerId);
    }
}
