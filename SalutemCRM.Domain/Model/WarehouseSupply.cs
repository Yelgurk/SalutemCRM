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

    [Required]
    public int? WarehouseItemForeignKey { get; set; }
    [ForeignKey("WarehouseItemForeignKey")]
    public WarehouseItem? WarehouseItem { get; set; }

    [Required]
    public int? WarehouseOrderForeignKey { get; set; }
    [ForeignKey("WarehouseOrderForeignKey")]
    public WarehouseOrder? WarehouseOrder { get; set; }

    [Required]
    public Delivery_Status? DeliveryStatus { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Code { get; set; }
    
    [Required]
    public double? PriceTotal { get; set; }
    
    [Required]
    public double? Count { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    public List<MaterialFlow> MaterialFlows { get; set; } = new();
}