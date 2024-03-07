﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class MaterialFlow
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int WarehouseSupplyForeignKey { get; set; }
    [ForeignKey("WarehouseSupplyForeignKey")]
    public WarehouseSupply WarehouseSupply { get; set; } = null!;

    public int? ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture? Manufacture { get; set; }

    public int? OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }

    public int? CustomerServiceForeignKey { get; set; }
    [ForeignKey("CustomerServiceForeignKey")]
    public CustomerServiceOrder? CustomerServiceOrder { get; set; }

    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }

    [Required]
    public double? Count { get; set; }
}
