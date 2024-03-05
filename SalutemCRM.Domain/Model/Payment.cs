using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? WarehouseOrderForeignKey { get; set; }
    [ForeignKey("WarehouseOrderForeignKey")]
    public WarehouseOrder? WarehouseOrder { get; set; }

    [Required]
    public int OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }

    [Required]
    public int CustomerServiceOrderForeignKey { get; set; }
    [ForeignKey("CustomerServiceOrderForeignKey")]
    public CustomerServiceOrder? CustomerServiceOrder { get; set; }

    [Required]
    [StringLength(200)]
    public string? Currency { get; set; }

    [Required]
    public double UnitToBYNConversion { get; set; }

    [Required]
    public double? PaymentValue { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? RecordDT { get; set; }
}