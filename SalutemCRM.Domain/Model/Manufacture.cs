﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class Manufacture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int? ProductTemplateForeignKey { get; set; }
    [ForeignKey("ProductTemplateForeignKey")]
    public ProductTemplate? ProductTemplate { get; set; }

    public int? OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? Code { get; set; }
    
    [Required]
    public Delivery_Status? DeliveryStatus { get; set; }
    
    [Required]
    public Task_Status? TaskStatus { get; set; }

    public List<ManufactureSupply>? ManufactureSupplies { get; set; } = new();
    public List<ManufactureProcess>? ManufactureProcesses { get; set; } = new();
    public List<CustomerService>? CustomerServices { get; set; } = new();
}