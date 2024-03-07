using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class OfficeOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ManagerForeignKey { get; set; }
    [ForeignKey("ManagerForeignKey")]
    public User Manager { get; set; } = null!;

    public int? ClientForeignKey { get; set; }
    [ForeignKey("ClientForeignKey")]
    public Client? Client { get; set; }

    public Order_Type OrderType { get; set; }
    public Payment_Status PaymentAgreement { get; set; }
    public Payment_Status PaymentStatus { get; set; }

    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }

    [StringLength(200)]
    public string Currency { get; set; } = null!;

    public double UnitToBYNConversion { get; set; }
    public double PriceRequired { get; set; }
    public double PriceTotal { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime ShipmentDeadlineDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? ShipmentDT { get; set; }

    public List<Manufacture> Manufactures { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
    public List<MaterialFlow> MaterialFlows { get; set; } = new();
    public List<FileAttach> FileAttachs { get; set; } = new();

    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}
