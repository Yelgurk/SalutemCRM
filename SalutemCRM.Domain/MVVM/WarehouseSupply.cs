using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseSupply
{
    [NotMapped]
    public double OrderPriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }

    [NotMapped]
    public double OrderPriceSingleBYN { get => OrderPriceTotalBYN / OrderCount; }

    [NotMapped]
    public double InStockPriceTotalBYN { get => OrderPriceTotalBYN / OrderCount * InStockCount; }
}