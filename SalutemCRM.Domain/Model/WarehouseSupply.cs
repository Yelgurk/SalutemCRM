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

    public int WarehouseItemForeignKey { get; set; }
    [ForeignKey("WarehouseItemForeignKey")]
    public WarehouseItem WarehouseItem { get; set; } = null!;

    public int WarehouseOrderForeignKey { get; set; }
    [ForeignKey("WarehouseOrderForeignKey")]
    public WarehouseOrder WarehouseOrder { get; set; } = null!;

    public Delivery_Status DeliveryStatus { get; set; }

    [MaxLength(200)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Currency { get; set; } = null!;

    public double UnitToBYNConversion { get; set; }
    public double PriceRequired { get; set; }
    public double PriceTotal { get; set; }

    public double Count { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    public List<MaterialFlow> MaterialFlows { get; set; } = new();

    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }

    [NotMapped]
    public double PriceSingleBYN { get => PriceTotalBYN / Count!; }
}