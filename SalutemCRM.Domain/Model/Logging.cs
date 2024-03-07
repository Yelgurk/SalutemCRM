
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class Logging
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }

    [MaxLength(200)]
    public string UserLogin { get; set; } = null!;

    [MaxLength(200)]
    public string UserFirstName { get; set; } = null!;

    [MaxLength(200)]
    public string UserLastName { get; set; } = null!;

    public string SQLQuery { get; set; } = null!;

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime RecordDT { get; set; }
}
