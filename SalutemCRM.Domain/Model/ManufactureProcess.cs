using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class ManufactureProcess
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeForeignKey { get; set; }
    [ForeignKey("EmployeeForeignKey")]
    public User Employee { get; set; } = null!;

    public int ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture Manufacture { get; set; } = null!;

    public int ManufactureDutyForeignKey { get; set; }
    [ForeignKey("ManufactureDutyForeignKey")]
    public ManufacturerDuty ManufacturerDuty { get; set; } = null!;

    public Task_Status TaskStatus { get; set; }
    public int Queue { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DeadlineDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? StartedDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? CompletedDT { get; set; }
}