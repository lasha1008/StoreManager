using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Query;

public class CustomerQueryServiceTests : QueryUnitTestsBase
{
    public CustomerQueryServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }


    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void Get(string firstname, string lastName, string userName)
    {
        Customer newCustomer = GetTestRecord(firstname, lastName, userName);
        _unitOfWork.CustomerRepository.Insert(newCustomer);
        _unitOfWork.SaveChanges();

        CustomerQueryService customerQueryService = new(_unitOfWork);

        Customer retrievedCustomer = customerQueryService.Get(newCustomer.Id);

        Assert.True(retrievedCustomer.Id == newCustomer.Id);
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void Set(string firstname, string lastName, string userName)
    {
        CustomerQueryService customerQueryService = new(_unitOfWork);

        Customer newCustomer = GetTestRecord(firstname, lastName, userName);
        List<Customer> expectedSet = new();
        expectedSet.Add(newCustomer);

        _unitOfWork.CustomerRepository.Insert(newCustomer);
        _unitOfWork.SaveChanges();

        var retrievedCustomers = customerQueryService.Set();

        Assert.True(expectedSet.Last().FirstName == retrievedCustomers.Last().FirstName &&
                    expectedSet.Last().LastName == retrievedCustomers.Last().LastName &&
                    expectedSet.Last().DisplayName == retrievedCustomers.Last().DisplayName
                    );
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void ExpressionSet(string firstname, string lastName, string userName)
    {
        CustomerQueryService customerQueryService = new(_unitOfWork);

        Customer newCustomer = GetTestRecord(firstname, lastName, userName);
        List<Customer> expectedSet = new();
        expectedSet.Add(newCustomer);

        _unitOfWork.CustomerRepository.Insert(newCustomer);
        _unitOfWork.SaveChanges();

        var retrievedCustomers = customerQueryService.Set(x => x.FirstName == firstname);

        Assert.True(expectedSet.Last().FirstName == retrievedCustomers.Last().FirstName &&
                    expectedSet.Last().LastName == retrievedCustomers.Last().LastName &&
                    expectedSet.Last().DisplayName == retrievedCustomers.Last().DisplayName
                   );
    }

    private static Customer GetTestRecord(string firstname, string lastName, string userName)
    {
        Customer customer = new()
        {
            FirstName = firstname,
            LastName = lastName,
            DisplayName = userName,
        };
        return customer;
    }
}
