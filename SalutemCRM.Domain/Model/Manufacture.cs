using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class Manufacture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }

    [Required]
    public Delivery_Status? DeliveryStatus { get; set; }

    [Required]
    public Task_Status? TaskStatus { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Model { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? ShipmentDT { get; set; }

    public List<MaterialFlow>? MaterialFlows { get; set; } = new();
    public List<ManufactureProcess>? ManufactureProcesses { get; set; } = new();
    public List<CustomerServiceOrder>? CustomerServiceOrders { get; set; } = new();
}