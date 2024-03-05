using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalutemCRM.Domain.Model;

public class ManufactureSupply
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? ManufactureForeignKey { get; set; }
    [ForeignKey("ManufactureForeignKey")]
    public Manufacture? Manufacture { get; set; }

    public int? WarehouseSupplyForeignKey { get; set; }
    [ForeignKey("WarehouseSupplyForeignKey")]
    public WarehouseSupply? WarehouseSupply { get; set; }

    [Required]
    public int? Count { get; set; }
}