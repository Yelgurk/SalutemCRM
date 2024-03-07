using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class CustomerServiceOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? OfficeManagerID { get; set; }

    public int? StockManagerForeignKey { get; set; }
    [ForeignKey("StockManagerForeignKey")]
    public User? StockManager { get; set; }

    public int? ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture? Manufacture { get; set; }

    public Payment_Status PaymentAgreement { get; set; }
    public Payment_Status PaymentStatus { get; set; }
    public Task_Status TaskStatus { get; set; }

    [MaxLength(200)]
    public string AdditionalInfo { get; set; } = null!;

    [StringLength(200)]
    public string Currency { get; set; } = null;

    public double UnitToBYNConversion { get; set; }
    public double PriceRequired { get; set; }
    public double PriceTotal { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DeadlineDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? StartedDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? CompletedDT { get; set; }

    public List<Payment> Payments { get; set; } = new();
    public List<MaterialFlow> MaterialFlows { get; set; } = new();
    public List<CustomerService> CustomerServices { get; set; } = new();
    public List<FileAttach> FileAttachs { get; set; } = new();

    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}
