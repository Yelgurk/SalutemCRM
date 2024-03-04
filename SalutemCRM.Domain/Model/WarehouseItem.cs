using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class WarehouseItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Code { get; set; }

    public List<WarehouseSupply> WarehouseSupplying { get; set; } = new();
}