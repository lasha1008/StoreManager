using System.ComponentModel;

namespace StoreManager.Web.Models;

public class ProductModel
{
    public int Id { get; set; }

    [DisplayName("Product Name")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }
}
