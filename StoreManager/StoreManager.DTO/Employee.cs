using System.ComponentModel.DataAnnotations;

namespace StoreManager.DTO;

public sealed class Employee : Person
{
    [MaxLength(30)]
    public string? Position { get; set; }

    public Employee? ReportsTo { get; set; }

    public AccountDetails? AccountDetails { get; set; }

    public ICollection<Employee>? ReportsFrom { get; set; }

    public ICollection<City>? Cities { get; set; }
}