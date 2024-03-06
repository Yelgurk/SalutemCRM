using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class Vendor
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

    public List<WarehouseOrder> WarehouseOrders { get; set; } = new();
}