using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.DTO;

public class AccountDetails : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(64)]
    public string Password { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public bool IsVerified {  get; set; } = false;

    [ForeignKey("Customer")]
    public int? Customer { get; set; }

    [ForeignKey("Employee")]
    public int? EmployeeId { get; set; }
}
