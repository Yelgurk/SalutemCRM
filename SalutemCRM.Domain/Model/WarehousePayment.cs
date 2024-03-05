using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class WarehousePayment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? WarehouseSupplyForeignKey { get; set; }
    [ForeignKey("WarehouseSupplyForeignKey")]
    public WarehouseSupply? WarehouseSupply { get; set; }

    [Required]
    public int CurrencyUnitForeignKey { get; set; }
    [ForeignKey("CurrencyUnitForeignKey")]
    public CurrencyUnit? CurrencyUnit { get; set; }

    [Required]
    public double UnitToBYNConversion { get; set; }

    [Required]
    public double? PaymentValue { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? RecordDT { get; set; }
}