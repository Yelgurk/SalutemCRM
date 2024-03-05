using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class ManufactureProcess
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    public int? UserForeignKey { get; set; }
    [ForeignKey("UserForeignKey")]
    public User? User { get; set; }

    [Required]
    public int ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture? Manufacture { get; set; }

    [Required]
    public int ManufactureDutyForeignKey { get; set; }
    [ForeignKey("ManufactureDutyForeignKey")]
    public ManufacturerDuty? ManufacturerDuty { get; set; }

    [Required]
    public int Queue { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DeadlineDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? Started { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? Finished { get; set; }

    [Required]
    public Task_Status Status { get; set; }
}