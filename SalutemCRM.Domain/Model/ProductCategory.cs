using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public class ProductCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public string Name { get; set; } = null!;

    public int Deep { get; set; }

    public int? ParentCategoryForeignKey { get; set; }
    [ForeignKey("ParentCategoryForeignKey")]
    public ProductCategory? ParentCategory { get; set; }

    public List<ProductCategory> SubCategories { get; set; } = new();
    public List<ProductTemplate> ProductTemplates { get; set; } = new();
}