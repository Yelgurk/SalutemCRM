using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ProductTemplate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Model { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    public List<ProductSchema> ProductSchemas { get; set; } = new();
    public List<Manufacture> Manufactures { get; set; } = new();
}