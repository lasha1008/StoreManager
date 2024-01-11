using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Repositories;

internal sealed class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(StoreManagerDbContext context) : base(context) { }

    public void DeleteAccount(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        AccountDetails? accountDetails = _context.AccountDetails.Find(customer.Id);
        _context.AccountDetails.Remove(accountDetails!);
    }
}