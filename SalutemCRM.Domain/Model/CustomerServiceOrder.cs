using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class CustomerServiceOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? StockManagerForeignKey { get; set; }
    [ForeignKey("StockManagerForeignKey")]
    public User? StockManager { get; set; }

    [Required]
    public int? ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture? Manufacture { get; set; }

    [Required]
    public Payment_Status? PaymentAgreement { get; set; }

    [Required]
    public Payment_Status? PaymentStatus { get; set; }

    [Required]
    public Task_Status? TaskStatus { get; set; }

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
    public DateTime? DeadlineDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? StartedDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? CompletedDT { get; set; }

    public List<Payment>? Payments { get; set; } = new();
    public List<MaterialFlow>? MaterialFlows { get; set; } = new();
    public List<CustomerService>? CustomerServices { get; set; } = new();
}
