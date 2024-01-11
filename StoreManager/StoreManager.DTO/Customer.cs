using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.DTO;

public sealed class Customer : Person
{
    [MaxLength(20)]
    public string DisplayName { get; set; } = null!;

    public string? VerificationToken { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TokenExpireTime { get; set; }

    public AccountDetails? AccountDetails { get; set; }
}
