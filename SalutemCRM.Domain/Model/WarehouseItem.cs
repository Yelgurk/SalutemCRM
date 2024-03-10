using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseItem : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _warehouseCategoryForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    [NotMapped]
    [ObservableProperty]
    private string _code = null!;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;

    [NotMapped]
    [ObservableProperty]
    private double _countRequired;



    [NotMapped]
    [ObservableProperty]
    private WarehouseCategory? _category;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _warehouseSupplying = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<ProductSchema> _productSchemas = new();
}