using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain.Model;

public partial class ProductSchema : ClonableObservableObject<ProductSchema>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _productTemplateForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _warehouseItemForeignKey;

    [NotMapped]
    [ObservableProperty]
    private double _count;



    [NotMapped]
    [ObservableProperty]
    private ProductTemplate? _productTemplate;

    [NotMapped]
    [ObservableProperty]
    private WarehouseItem? _warehouseItem;
}