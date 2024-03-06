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

    public Delivery_Status DeliveryStatus { get; set; }
    public Task_Status TaskStatus { get; set; }

    [MaxLength(200)]
    public string Code { get; set; } = null!;

    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(200)]
    public string Model { get; set; } = null!;

    [MaxLength(200)]
    public string AdditionalInfo { get; set; } = null!;

    [Column(TypeName = "datetime2")]
    public DateTime? ShipmentDT { get; set; }

    public List<MaterialFlow> MaterialFlows { get; set; } = new();
    public List<ManufactureProcess> ManufactureProcesses { get; set; } = new();
    public List<CustomerServiceOrder> CustomerServiceOrders { get; set; } = new();
}