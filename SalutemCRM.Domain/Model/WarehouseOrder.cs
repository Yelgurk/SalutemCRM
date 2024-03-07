using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class WarehouseOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int StorekeeperForeignKey { get; set; }
    [ForeignKey("StorekeeperForeignKey")]
    public User Storekeeper { get; set; } = null!;

    public int? VendorForeignKey { get; set; }
    [ForeignKey("VendorForeignKey")]
    public Vendor? Vendor { get; set; }

    public Payment_Status PaymentAgreement { get; set; }
    public Payment_Status? PaymentStatus { get; set; }

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
    public DateTime? ReceivedDT { get; set; }

    public List<Payment> Payments { get; set; } = new();
    public List<WarehouseSupply> WarehouseSupplies { get; set; } = new();
    public List<FileAttach> FileAttachs { get; set; } = new();

    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}
