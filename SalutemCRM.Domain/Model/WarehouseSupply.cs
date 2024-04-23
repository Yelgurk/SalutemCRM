using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseSupply : ClonableObservableObject<WarehouseSupply>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _warehouseItemForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Delivery_Status _deliveryStatus;

    [NotMapped]
    [ObservableProperty]
    private string _vendorName = null!;

    [NotMapped]
    [ObservableProperty]
    private string? _vendorCode;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OrderPriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(OrderPriceSingleBYN))]
    [NotifyPropertyChangedFor(nameof(InStockPriceTotalBYN))]
    private string _currency = "BYN";

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OrderPriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(OrderPriceSingleBYN))]
    [NotifyPropertyChangedFor(nameof(InStockPriceTotalBYN))]
    private double _unitToBYNConversion = 1.0;

    [NotMapped]
    [ObservableProperty]
    private double _priceRequired;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OrderPriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(OrderPriceSingleBYN))]
    [NotifyPropertyChangedFor(nameof(InStockPriceTotalBYN))]
    private double _priceTotal;

    [NotMapped]
    [ObservableProperty]
    private double _orderCount;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OrderPriceTotalBYN))]
    [NotifyPropertyChangedFor(nameof(OrderPriceSingleBYN))]
    [NotifyPropertyChangedFor(nameof(InStockPriceTotalBYN))]
    private double _inStockCount;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;



    [NotMapped]
    [ObservableProperty]
    private WarehouseItem _warehouseItem = null!;

    [NotMapped]
    [ObservableProperty]
    private Order _order = null!;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();
}