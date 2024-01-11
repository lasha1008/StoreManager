using Microsoft.EntityFrameworkCore.Storage;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreManagerDbContext _context;
    private readonly Lazy<ICategoryRepository> _categoryRepositoryLazy;
    private readonly Lazy<ICityRepository> _cityRepositoryLazy;
    private readonly Lazy<ICountryRepository> _countryRepositoryLazy;
    private readonly Lazy<ICustomerRepository> _customerRepositoryLazy;
    private readonly Lazy<IEmployeeRepository> _employeeRepositoryLazy;
    private readonly Lazy<IOrderDetailsRepository> _orderDetailsRepositoryLazy;
    private readonly Lazy<IOrderRepository> _orderRepositoryLazy;
    private readonly Lazy<IProductRepository> _productRepositoryLazy;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(StoreManagerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _categoryRepositoryLazy = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
        _cityRepositoryLazy = new Lazy<ICityRepository>(() => new CityRepository(context));
        _countryRepositoryLazy = new Lazy<ICountryRepository>(() => new CountryRepository(context));
        _customerRepositoryLazy = new Lazy<ICustomerRepository>(() => new CustomerRepository(context));
        _employeeRepositoryLazy = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
        _orderDetailsRepositoryLazy = new Lazy<IOrderDetailsRepository>(() => new OrderDetailsRepository(context));
        _orderRepositoryLazy = new Lazy<IOrderRepository>(() => new OrderRepository(context));
        _productRepositoryLazy = new Lazy<IProductRepository>(() => new ProductRepository(context));
    }

    public ICategoryRepository CategoryRepository => _categoryRepositoryLazy.Value;

    public ICityRepository CityRepository => _cityRepositoryLazy.Value;

    public ICountryRepository CountryRepository => _countryRepositoryLazy.Value;

    public ICustomerRepository CustomerRepository => _customerRepositoryLazy.Value;

    public IEmployeeRepository EmployeeRepository => _employeeRepositoryLazy.Value;

    public IOrderDetailsRepository OrderDetailsRepository => _orderDetailsRepositoryLazy.Value;

    public IOrderRepository OrderRepository => _orderRepositoryLazy.Value;

    public IProductRepository ProductRepository => _productRepositoryLazy.Value;

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void BeginTransaction() => _transaction = _context.Database.BeginTransaction();

    public void CommitTransaction()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _context.Dispose();
        _transaction?.Dispose();
    }
}
