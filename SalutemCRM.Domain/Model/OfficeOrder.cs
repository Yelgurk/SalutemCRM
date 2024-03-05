using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class OfficeOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? ManagerForeignKey { get; set; }
    [ForeignKey("ManagerForeignKey")]
    public User? Manager { get; set; }

    public int? ClientForeignKey { get; set; }
    [ForeignKey("ClientForeignKey")]
    public Client? Client { get; set; }

    [Required]
    public Payment_Status? PaymentAgreement { get; set; }

    [Required]
    public Payment_Status? PaymentStatus { get; set; }

    [Required]
    public double? PriceManufacture { get; set; }

    [Required]
    public double? PriceTotal { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? ShipmentDeadline { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? ShippmentCompleted { get; set; }

    public List<Manufacture>? Manufactures { get; set; } = new();
    public List<OfficeOrderPayment>? OfficeOrderPayments { get; set; } = new();
}
