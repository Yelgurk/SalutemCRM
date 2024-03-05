using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SalutemCRM.Domain.Modell;
using System.ComponentModel;

namespace SalutemCRM.Domain.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? UserRoleForeignKey { get; set; }
    [ForeignKey("UserRoleForeignKey")]
    public UserRole? UserRole { get; set; }

    [DefaultValue(true)]
    public bool IsActive { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Login { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string? PasswordMD5 { get; set; }

    [Required]
    [MaxLength(200)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(200)]
    public string? LastName { get; set; }

    public List<ManufactureProcess>? ManufactureProcesses { get; set; } = new();
    public List<OfficeOrder>? OfficeOrders { get; set; } = new();
    public List<WarehouseOrder>? WarehouseOrders { get; set; } = new();
    public List<CustomerServiceOrder>? CustomerServiceOrders { get; set; } = new();
}
