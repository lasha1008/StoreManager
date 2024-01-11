namespace StoreManager.Facade.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    ICityRepository CityRepository { get; }
    ICountryRepository CountryRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    IOrderDetailsRepository OrderDetailsRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductRepository ProductRepository { get; }

    int SaveChanges();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
