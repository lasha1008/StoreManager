using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;

namespace StoreManager.Tests.Repositories;

public class EmployeeRepositoryTests : RepositoryUnitTestBase
{
    public EmployeeRepositoryTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void Insert(string firstname, string lastName, string userName, string password)
    {
        Employee employee = GetTestRecord(firstname, lastName, userName, password);
        _unitOfWork.EmployeeRepository.Insert(employee);
        _unitOfWork.SaveChanges();

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
            _unitOfWork.EmployeeRepository.Insert(employee);
            _unitOfWork.SaveChanges();
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
        _unitOfWork.EmployeeRepository.Insert(newEmployee);
        _unitOfWork.SaveChanges();

        newEmployee.AccountDetails!.Username = $"New {userName}";
        _unitOfWork.EmployeeRepository.Update(newEmployee);
        _unitOfWork.SaveChanges();

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
            _unitOfWork.EmployeeRepository.Update(newEmployee);
            _unitOfWork.SaveChanges();
        });
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void DeleteByObject(string firstname, string lastName, string userName, string password)
    {
        Employee employee = GetTestRecord(firstname, lastName, userName, password);
        _unitOfWork.EmployeeRepository.Insert(employee);
        _unitOfWork.SaveChanges();

        _unitOfWork.EmployeeRepository.Delete(employee);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.EmployeeRepository.Set(x => x.Id == employee.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void DeleteById(string firstname, string lastName, string userName, string password)
    {
        Employee employee = GetTestRecord(firstname, lastName, userName, password);
        _unitOfWork.EmployeeRepository.Insert(employee);
        _unitOfWork.SaveChanges();

        _unitOfWork.EmployeeRepository.Delete(employee.Id);
        _unitOfWork.SaveChanges();

        Assert.Null(_unitOfWork.EmployeeRepository.Set(x => x.Id == employee.Id).SingleOrDefault());
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1", "UserName 1", "Password 1")]
    [InlineData("Firstname 2", "LastName 2", "UserName 2", "Password 2")]
    [InlineData("Firstname 3", "LastName 3", "UserName 3", "Password 3")]
    [InlineData("Firstname 4", "LastName 4", "UserName 4", "Password 4")]
    public void GetById(string firstname, string lastName, string userName, string password)
    {
        Employee newEmployee = GetTestRecord(firstname, lastName, userName, password);
        _unitOfWork.EmployeeRepository.Insert(newEmployee);
        _unitOfWork.SaveChanges();

        Employee retrievedEmployee = _unitOfWork.EmployeeRepository.Get(newEmployee.Id);

        Assert.True(retrievedEmployee.Id == newEmployee.Id);
    }

    [Theory]
    [InlineData("Firstname 100", "LastName 100", "UserName 100", "Password 100")]
    [InlineData("Firstname 101", "LastName 101", "UserName 101", "Password 101")]
    [InlineData("Firstname 102", "LastName 102", "UserName 102", "Password 102")]
    [InlineData("Firstname 103", "LastName 103", "UserName 103", "Password 103")]
    public void Set(string firstname, string lastName, string userName, string password)
    {
        Employee newEmployee = GetTestRecord(firstname, lastName, userName, password);
        List<Employee> expectedSet = new();
        expectedSet.Add(newEmployee);

        _unitOfWork.EmployeeRepository.Insert(newEmployee);
        _unitOfWork.SaveChanges();

        var retrievedEmployees = _unitOfWork.EmployeeRepository.Set();

        Assert.True(expectedSet.Last().FirstName == retrievedEmployees.Last().FirstName &&
                    expectedSet.Last().LastName == retrievedEmployees.Last().LastName &&
                    expectedSet.Last().AccountDetails!.Username == retrievedEmployees.Last().AccountDetails!.Username &&
                    expectedSet.Last().AccountDetails!.Password == retrievedEmployees.Last().AccountDetails!.Password
        );
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