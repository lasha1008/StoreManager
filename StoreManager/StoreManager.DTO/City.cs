using System.ComponentModel.DataAnnotations;

namespace StoreManager.DTO;

public class City : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(25)]
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    [Required]
    public Country Country { get; set; } = null!;

    public ICollection<Employee>? Employees { get; set; }
}