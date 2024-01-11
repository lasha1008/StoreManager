using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Command;

public class EmployeeCommandServiceTests : CommandUnitTestsBase
{
    public EmployeeCommandServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void Insert(string firstname, string lastName, string userName, string password)
    {
        Employee employee = GetTestRecord(firstname, lastName, userName, password);

        EmployeeCommandService employeeCommandService = new(_unitOfWork);
        employeeCommandService.Insert(employee);

        Assert.True(employee.Id > 0);
    }

    [Theory]
    [InlineData("Firstname 5", "LastName 5", "UserName 5", "Password 5")]
    [InlineData("Firstname 6", "LastName 6", "UserName 6", "Password 6")]
    [InlineData("Firstname 7", "LastName 7", "UserName 7", "Password 7")]
    [InlineData("Firstname 8", "LastName 8", "UserName 8", "Password 8")]
    public void NotInserted(string firstname, string lastName, string userName, string password)
    {
        Employee employee = GetTestRecord(firstname, lastName, userName, password);
        employee.Id = 1;

        Assert.Throws<ArgumentException>(() =>
        {
            EmployeeCommandService employeeCommandService = new(_unitOfWork);
            employeeCommandService.Insert(employee);
        });
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void Update(string firstname, string lastName, string userName, string password)
    {
        Employee newEmployee = GetTestRecord(firstname, lastName, userName, password);

        EmployeeCommandService employeeCommandService = new(_unitOfWork);
        employeeCommandService.Insert(newEmployee);

        newEmployee.AccountDetails!.Username = $"New {userName}";

        employeeCommandService.Update(newEmployee);

        Employee updatedEmployee = _unitOfWork.EmployeeRepository.Set(x => x.Id == newEmployee.Id).Single();

        Assert.True(updatedEmployee.AccountDetails!.Username == newEmployee.AccountDetails.Username);
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void NotUpdated(string firstname, string lastName, string userName, string password)
    {
        Employee newEmployee = GetTestRecord(firstname, lastName, userName, password);

        Assert.Throws<DbUpdateConcurrencyException>(() =>
        {
            EmployeeCommandService employeeCommandService = new(_unitOfWork);
            employeeCommandService.Update(newEmployee);
        });
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void Delete(string firstname, string lastName, string userName, string password)
    {
        Employee employee = GetTestRecord(firstname, lastName, userName, password);

        EmployeeCommandService employeeCommandService = new(_unitOfWork);
        employeeCommandService.Insert(employee);

        employeeCommandService.Delete(employee);

        Assert.True(_unitOfWork.EmployeeRepository.Set(x => x.Id == employee.Id).Single().IsDeleted);
    }

    private static Employee GetTestRecord(string firstname, string lastName, string userName, string password)
    {
        Employee employee = new()
        {
            FirstName = firstname,
            LastName = lastName,
            AccountDetails = new AccountDetails
            {
                Username = userName,
                Password = password
            }
        };
        return employee;
    }
}
