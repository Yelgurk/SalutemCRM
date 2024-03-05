using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class WarehouseItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? WarehouseCategoryForeignKey { get; set; }
    [ForeignKey("WarehouseCategoryForeignKey")]
    public WarehouseCategory? Category { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Code { get; set; }

    [Required]
    public double? CountRequired { get; set; }

    public List<WarehouseSupply> WarehouseSupplying { get; set; } = new();
    public List<ProductSchema> ProductSchemas { get; set; } = new();
}