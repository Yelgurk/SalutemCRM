using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class WarehouseSupply
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int? WarehouseItemForeignKey { get; set; }
    [ForeignKey("WarehouseItemForeignKey")]
    public WarehouseItem? WarehouseItem { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Code { get; set; }
    
    [Required]
    public double? PriceTotal { get; set; }
    
    [Required]
    public int? Count { get; set; }
    
    [Required]
    public Payment_Status? PaymentStatus { get; set; }
    
    [Required]
    public Delivery_Status? DeliveryStatus { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    public List<ManufactureSupply>? ManufactureSupplies { get; set; } = new();
    public List<WarehousePayment>? WarehousePayments { get; set; } = new();
}