using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class ManufacturerDuty
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public string Name { get; set; } = null!;

    public List<ManufactureProcess> ManufactureProcesses { get; set; } = new();
}
