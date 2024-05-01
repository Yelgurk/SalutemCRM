using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class ProductTemplate : ClonableObservableObject<ProductTemplate>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _manufactureCategoryForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    [NotMapped]
    [ObservableProperty]
    private bool _haveSerialNumber = false;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;



    [NotMapped]
    [ObservableProperty]
    private ProductCategory? _category;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<ProductSchema> _productSchemas = new();
}