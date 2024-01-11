using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Repositories;
using StoreManager.Services;

namespace StoreManager.Tests.Services.Query;

public class EmployeeQueryServiceTests : QueryUnitTestsBase
{
    public EmployeeQueryServiceTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }


    [Theory]
    [InlineData("Firstname 1", "LastName 1")]
    [InlineData("Firstname 2", "LastName 2")]
    [InlineData("Firstname 3", "LastName 3")]
    [InlineData("Firstname 4", "LastName 4")]
    public void Get(string firstname, string lastName)
    {
        Employee newEmployee = GetTestRecord(firstname, lastName);
        _unitOfWork.EmployeeRepository.Insert(newEmployee);
        _unitOfWork.SaveChanges();

        EmployeeQueryService employeeQueryService = new(_unitOfWork);

        Employee retrievedEmployee = employeeQueryService.Get(newEmployee.Id);

        Assert.True(retrievedEmployee.Id == newEmployee.Id);
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1")]
    [InlineData("Firstname 2", "LastName 2")]
    [InlineData("Firstname 3", "LastName 3")]
    [InlineData("Firstname 4", "LastName 4")]
    public void Set(string firstname, string lastName)
    {
        EmployeeQueryService employeeQueryService = new(_unitOfWork);

        Employee newEmployee = GetTestRecord(firstname, lastName);
        List<Employee> expectedSet = new();
        expectedSet.Add(newEmployee);

        _unitOfWork.EmployeeRepository.Insert(newEmployee);
        _unitOfWork.SaveChanges();

        var retrievedEmployees = employeeQueryService.Set();

        Assert.True(expectedSet.Last().FirstName == retrievedEmployees.Last().FirstName &&
                    expectedSet.Last().LastName == retrievedEmployees.Last().LastName
                    );
    }

    [Theory]
    [InlineData("Firstname 1", "LastName 1")]
    [InlineData("Firstname 2", "LastName 2")]
    [InlineData("Firstname 3", "LastName 3")]
    [InlineData("Firstname 4", "LastName 4")]
    public void ExpressionSet(string firstname, string lastName)
    {
        EmployeeQueryService employeeQueryService = new(_unitOfWork);

        Employee newEmployee = GetTestRecord(firstname, lastName);
        List<Employee> expectedSet = new();
        expectedSet.Add(newEmployee);

        _unitOfWork.EmployeeRepository.Insert(newEmployee);
        _unitOfWork.SaveChanges();

        var retrievedEmployees = employeeQueryService.Set(x => x.FirstName == firstname);

        Assert.True(expectedSet.Last().FirstName == retrievedEmployees.Last().FirstName &&
                    expectedSet.Last().LastName == retrievedEmployees.Last().LastName
                   );
    }

    private static Employee GetTestRecord(string firstname, string lastName)
    {
        Employee employee = new()
        {
            FirstName = firstname,
            LastName = lastName
        };
        return employee;
    }
}
