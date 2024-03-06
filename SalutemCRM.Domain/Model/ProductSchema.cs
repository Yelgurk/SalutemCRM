﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ProductSchema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int? ProductTemplateForeignKey { get; set; }
    [ForeignKey("ProductTemplateForeignKey")]
    public ProductTemplate? ProductTemplate { get; set; }

    [Required]
    public int? WarehouseItemForeignKey { get; set; }
    [ForeignKey("WarehouseItemForeignKey")]
    public WarehouseItem? WarehouseItem { get; set; }
    
    [Required]
    public double Count { get; set; }
}