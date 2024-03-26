using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseSupply : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _warehouseItemForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _warehouseOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Delivery_Status _deliveryStatus;

    [NotMapped]
    [ObservableProperty]
    private string? _vendorCode;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(PriceSingleBYN))]
    private string _currency = null!;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(PriceSingleBYN))]
    private double _unitToBYNConversion;

    [NotMapped]
    [ObservableProperty]
    private double _priceRequired;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(PriceSingleBYN))]
    private double _priceTotal;

    [NotMapped]
    [ObservableProperty]
    private double _orderCount;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(PriceSingleBYN))]
    private double _inStockCount;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;



    [NotMapped]
    [ObservableProperty]
    private WarehouseItem _warehouseItem = null!;

    [NotMapped]
    [ObservableProperty]
    private WarehouseOrder _warehouseOrder = null!;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();



    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }

    [NotMapped]
    public double PriceSingleBYN { get => PriceTotalBYN / InStockCount; }
}