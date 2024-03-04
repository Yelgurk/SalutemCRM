using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class WarehouseSupply
{
    [Key]
    public int Id { get; set; }
    public int? WarehouseItemForeignKey { get; set; }
    [ForeignKey("WarehouseItemForeignKey")]
    public WarehouseItem? WarehouseItem { get; set; }
    public string? Code { get; set; }
    public double PriceSingle { get; set; }
    public int CountOrdered { get; set; }
    public int CountAvailable { get; set; }
    public Payment_Status PaymentStatus { get; set; }
    public Delivery_Status DeliveryStatus { get; set; }

    //public WarehouseSupply (int CountOrdered) => this.CountOrdered = CountOrdered;
    //public virtual WarehouseSupply Usage() => this;
}