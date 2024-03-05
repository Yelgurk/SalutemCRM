using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class CustomerService
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int EmployeeForeignKey { get; set; }
    [ForeignKey("EmployeeForeignKey")]
    public User? Employee { get; set; }

    [Required]
    public int ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture? Manufacture { get; set; }

    [Required]
    [MaxLength(200)]
    public string? AdditionalInfo { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? RecordDT { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? FinishedDT { get; set; }

    [Required]
    public Task_Status? Status { get; set; }
}
