using System.ComponentModel.DataAnnotations;

namespace StoreManager.DTO;

public class Country : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public ICollection<City>? Cities { get; set; }
}