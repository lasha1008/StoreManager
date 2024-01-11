using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Command;

public class CustomerCommandServiceTests : CommandUnitTestsBase
{
    public CustomerCommandServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void Insert(string firstname, string lastName, string userName)
    {
        Customer customer = GetTestRecord(firstname, lastName, userName);

        CustomerCommandService customerCommandService = new(_unitOfWork);
        customerCommandService.Insert(customer);
       
        Assert.True(customer.Id > 0);
    }

    [Theory]
    [InlineData("Firstname 9", "LastName 9", "UserName 9")]
    [InlineData("Firstname 10", "LastName 10", "UserName 10")]
    [InlineData("Firstname 11", "LastName 11", "UserName 11")]
    [InlineData("Firstname 12", "LastName 12", "UserName 12")]
    public void NotInserted(string firstname, string lastName, string userName)
    {
        Customer customer = GetTestRecord(firstname, lastName, userName);
        customer.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            CustomerCommandService customerCommandService = new(_unitOfWork);
            customerCommandService.Insert(customer);
        });
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void Update(string firstname, string lastName, string userName)
    {
        Customer newCustomer = GetTestRecord(firstname, lastName, userName);

        CustomerCommandService customerCommandService = new(_unitOfWork);
        customerCommandService.Insert(newCustomer);

        newCustomer.DisplayName = $"New {userName}";

        customerCommandService.Update(newCustomer);

        Customer updatedCustomer = _unitOfWork.CustomerRepository.Set(x => x.Id == newCustomer.Id).Single();

        Assert.True(updatedCustomer.DisplayName == newCustomer.DisplayName);
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void NotUpdated(string firstname, string lastName, string userName)
    {
        Customer newCustomer = GetTestRecord(firstname, lastName, userName);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            CustomerCommandService customerCommandService = new(_unitOfWork);
            customerCommandService.Update(newCustomer);
        });
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4")]
    public void Delete(string firstname, string lastName, string userName)
    {
        Customer customer = GetTestRecord(firstname, lastName, userName);

        CustomerCommandService customerCommandService = new(_unitOfWork);
        customerCommandService.Insert(customer);

        customerCommandService.Delete(customer);

        Assert.True(_unitOfWork.CustomerRepository.Set(x => x.Id == customer.Id).Single().IsDeleted);
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
