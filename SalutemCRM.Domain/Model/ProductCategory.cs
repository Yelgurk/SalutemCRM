using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ProductCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    public int? ParentCategoryForeignKey { get; set; }
    [ForeignKey("ParentCategoryForeignKey")]
    public ProductCategory? ParentCategory { get; set; }

    public List<ProductCategory> SubCategories { get; set; } = new();
    public List<ProductTemplate> ProductTemplates { get; set; } = new();
}