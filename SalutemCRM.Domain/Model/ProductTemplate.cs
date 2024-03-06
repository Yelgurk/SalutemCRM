using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ProductTemplate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int? ManufactureCategoryForeignKey { get; set; }
    [ForeignKey("ManufactureCategoryForeignKey")]
    public ProductCategory? Category { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Model { get; set; }

    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }

    public List<ProductSchema> ProductSchemas { get; set; } = new();
}