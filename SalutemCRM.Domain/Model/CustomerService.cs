using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class CustomerService
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeForeignKey { get; set; }
    [ForeignKey("EmployeeForeignKey")]
    public User Employee { get; set; } = null!;

    public int CustomerServiceOrderForeignKey { get; set; }
    [ForeignKey("CustomerServiceOrderForeignKey")]
    public CustomerServiceOrder CustomerServiceOrder { get; set; } = null!;

    public Task_Status TaskStatus { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? StartedDT { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? CompletedDT { get; set; }
}
