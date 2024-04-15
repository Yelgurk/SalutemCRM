using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseCategory : ClonableObservableObject<WarehouseCategory>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    [NotMapped]
    [ObservableProperty]
    private int _deep;

    [NotMapped]
    [ObservableProperty]
    private int? _parentCategoryForeignKey;



    [NotMapped]
    [ObservableProperty]
    private WarehouseCategory? _parentCategory;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<WarehouseCategory> _subCategories = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<WarehouseItem> _warehouseItems = new();
}