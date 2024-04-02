using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain.Model;

public partial class ProductCategory : ObservableObject
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
    private ProductCategory? _parentCategory;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<ProductCategory> _subCategories = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<ProductTemplate> _productTemplates = new();

    public object Clone() { return this.MemberwiseClone(); }
}