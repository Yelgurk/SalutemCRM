using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class CurrencyUnit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    public List<WarehousePayment>? WarehousePayments { get; set; } = new();
    public List<OfficeOrderPayment>? OfficeOrderPayments { get; set; } = new();
}