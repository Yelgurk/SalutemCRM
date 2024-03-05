using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class WarehouseCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    public int? ParentCategoryForeignKey { get; set; }
    [ForeignKey("ParentCategoryForeignKey")]
    public WarehouseCategory? ParentCategory { get; set; }
    
    public List<WarehouseCategory>? SubCategories { get; set; }
    public List<WarehouseItem>? WarehouseItems{ get; set; }
}
