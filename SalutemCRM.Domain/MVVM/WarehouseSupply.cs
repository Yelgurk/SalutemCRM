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
    public double InStockPriceTotalBYN { get => OrderPriceSingleBYN * InStockCount; }

    [NotMapped]
    public double PriceTotalInput = 0;

    [NotMapped]
    private double _priceForSingleItemInput = 0.0;

    [NotMapped]
    public double PriceForSingleItemInput
    {
        get => _priceForSingleItemInput;
        set
        {
            _priceForSingleItemInput = value;
            PriceTotalInput = value * OrderCount;
            OnPropertyChanged(nameof(PriceTotalInput));
            OnPropertyChanged(nameof(PriceForSingleItemInput));
        }
    }

    partial void OnOrderCountChanged(double value) => PriceTotalInput = PriceForSingleItemInput * value;
}