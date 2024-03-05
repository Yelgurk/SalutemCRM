using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public class FileAttach
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int? OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }

    [Required]
    public int? WarehouseOrderForeignKey { get; set; }
    [ForeignKey("WarehouseOrderForeignKey")]
    public WarehouseOrder? WarehouseOrder { get; set; }

    [Required]
    public int? CustomerServiceOrderForeignKey { get; set; }
    [ForeignKey("CustomerServiceOrderForeignKey")]
    public CustomerServiceOrder? CustomerServiceOrder { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? RecordDT { get; set; }

    [Required]
    public string? FileName { get; set; }
}