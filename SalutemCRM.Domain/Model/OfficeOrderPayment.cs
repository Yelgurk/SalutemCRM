
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class OfficeOrderPayment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }

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