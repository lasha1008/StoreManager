using System.ComponentModel.DataAnnotations;

namespace StoreManager.DTO;

public class Category : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Product>? Products { get; set; }
}