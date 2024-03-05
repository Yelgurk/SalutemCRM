
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class WarehouseSale
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int OfficeOrderForeignKey { get; set; }
    [ForeignKey("OfficeOrderForeignKey")]
    public OfficeOrder? OfficeOrder { get; set; }

    public int? WarehouseSupplyForeignKey { get; set; }
    [ForeignKey("WarehouseSupplyForeignKey")]
    public WarehouseSupply? WarehouseSupply { get; set; }

    [Required]
    public int? Count { get; set; }
}
