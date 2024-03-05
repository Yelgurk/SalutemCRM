using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ManufactureCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    public int? ParentCategoryForeignKey { get; set; }
    [ForeignKey("ParentCategoryForeignKey")]
    public ManufactureCategory? ParentCategory { get; set; }

    public List<ManufactureCategory>? SubCategories { get; set; }
    public List<ProductTemplate>? ProductTemplates { get; set; }
}