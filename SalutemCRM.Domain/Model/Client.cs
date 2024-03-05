
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Name { get; set; }

    [Required]
    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(200)]
    public string? AdditionalInfo { get; set; }

    public List<OfficeOrder>? OfficeOrders { get; set; } = new();
}
