using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.DTO;

public abstract class Person : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    [MaxLength(15)]
    [Column(TypeName = "varchar")]
    public string? Phone { get; set; }

    [MaxLength(25)]
    public string? Email { get; set; }

    [MaxLength(150)]
    public string? Address { get; set; }

    public bool IsDeleted { get; set; }
}
