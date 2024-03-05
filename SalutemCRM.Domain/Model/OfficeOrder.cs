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
    public Order_Type? OrderType { get; set; }

    [Required]
    public Payment_Status? PaymentAgreement { get; set; }

    [Required]
    public Payment_Status? PaymentStatus { get; set; }

    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }

    [Required]
    public double? PriceRequired { get; set; }

    [Required]
    public double? PriceTotal { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? ShipmentDeadlineDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? ShipmentDT { get; set; }

    public List<Manufacture>? Manufactures { get; set; } = new();
    public List<Payment>? Payments { get; set; } = new();
    public List<MaterialFlow>? MaterialFlows { get; set; } = new();
    public List<FileAttach>? FileAttachs { get; set; } = new();
}
