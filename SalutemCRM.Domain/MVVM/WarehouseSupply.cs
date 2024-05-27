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
    private double _orderBuilder_Count = 0;

    [NotMapped]
    private double _orderBuilder_PriceSingle = 0;

    [NotMapped]
    private double _orderBuilder_ToBYNConv = 1.00;

    [NotMapped]
    public double OrderBuilder_PriceTotal => OrderBuilder_Count * OrderBuilder_PriceSingle;

    [NotMapped]
    public double OrderBuilder_PriceTotalBYN => OrderBuilder_PriceTotal * OrderBuilder_ToBYNConv;

    [NotMapped]
    public double OrderBuilder_Count
    {
        get => _orderBuilder_Count;
        set => value
            .Do(x => _orderBuilder_Count = x)
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_Count)))
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceTotal)))
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceTotalBYN)));
    }

    [NotMapped]
    public double OrderBuilder_PriceSingle
    {
        get => _orderBuilder_PriceSingle;
        set => value
            .Do(x => _orderBuilder_PriceSingle = x)
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceSingle)))
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceTotal)))
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceTotalBYN)));
    }

    [NotMapped]
    public double OrderBuilder_ToBYNConv
    {
        get => _orderBuilder_ToBYNConv;
        set => value
            .Do(x => _orderBuilder_ToBYNConv = x)
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_ToBYNConv)))
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceTotal)))
            .Do(x => OnPropertyChanged(nameof(OrderBuilder_PriceTotalBYN)));
    }

    [NotMapped]
    public ObservableCollection<string> ScannedQrCodes { get; } = new();

    [NotMapped]
    private double _scannedCount = 0;

    [NotMapped]
    public double ScannedCount
    {
        get => _scannedCount;
        set
        {
            _scannedCount = value;
            OnPropertyChanged(nameof(ScannedCount));
        }
    }
}