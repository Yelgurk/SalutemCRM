using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseItem : ClonableObservableObject<WarehouseItem>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _warehouseCategoryForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string _innerName = null!;

    [NotMapped]
    [ObservableProperty]
    private string _innerCode = null!;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;

    [NotMapped]
    [ObservableProperty]
    private string _mesurementUnit = "";

    [NotMapped]
    [ObservableProperty]
    private double _countRequired;

    [NotMapped]
    [ObservableProperty]
    private WarehouseCategory? _category;



    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalInStockCount))]
    [NotifyPropertyChangedFor(nameof(CountInStockState))]
    [NotifyPropertyChangedFor(nameof(CountInStockStateToString))]
    [NotifyPropertyChangedFor(nameof(BYNPriceForSingleInStock))]
    [NotifyPropertyChangedFor(nameof(BYNPriceForTotalInStock))]
    private ObservableCollection<WarehouseSupply> _warehouseSupplying = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<ProductSchema> _productSchemas = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();
}