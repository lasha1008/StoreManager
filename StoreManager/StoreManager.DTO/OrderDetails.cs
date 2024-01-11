using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.DTO;

public class OrderDetails
{
    [Key]
    public int OrderId { get; set; }

    [Key]
    public int ProductId { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }

    public bool IsDeleted { get; set; }

    [Required]
    public Product Product { get; set; } = null!;

    [Required]
    public Order Order { get; set; } = null!;
}
