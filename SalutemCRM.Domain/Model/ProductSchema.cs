using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ProductSchema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int? ProductTemplateForeignKey { get; set; }
    [ForeignKey("ProductTemplateForeignKey")]
    public ProductTemplate? ProductTemplate { get; set; }
    
    public int? WarehouseItemForeignKey { get; set; }
    [ForeignKey("WarehouseItemForeignKey")]
    public WarehouseItem? WarehouseItem { get; set; }
    
    [Required]
    public int Count { get; set; }
}