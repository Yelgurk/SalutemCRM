using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ProductTemplate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? Model { get; set; }
    
    public string? Description { get; set; }

    public List<ProductSchema> ProductSchemas { get; set; } = new();
    public List<Manufacture> Manufactures { get; set; } = new();
}