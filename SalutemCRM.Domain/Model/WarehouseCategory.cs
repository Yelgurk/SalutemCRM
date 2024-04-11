using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseCategory : ObservableObject
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



    public WarehouseCategory() { }

    public WarehouseCategory(WarehouseCategory source)
    {
        this.Id = source.Id;
        this.Name = source.Name;
        this.Deep = source.Deep;
    }

    [NotMapped]
    public string NameHierarchyToZeroDeepParent => $"{this.Name} {ParentCategory?.Name ?? ""}";

    [NotMapped]
    public List<WarehouseCategory> ObjHierarchyToZeroDeepParent =>
        new List<WarehouseCategory>()
        .Do(x => x.Add(this))
        .DoInst(x => x.DoIf(y => y.AddRange(this.ParentCategory!.Clone().Do(z => { z.SubCategories.Clear(); z.SubCategories.Add(this); }).ObjHierarchyToZeroDeepParent), y => this.ParentCategory is not null));

    public void ObjHierarchyToLastChild(List<WarehouseCategory> list)
    {
        list
            .DoIf(x => this.SubCategories.Add(new(x.First())), x => x.Count > 0)?
            .Do(x => x.RemoveAt(0))
            .DoIf(x => this.SubCategories.First().ObjHierarchyToLastChild(x), x => x.Count > 0);
    }

    public WarehouseCategory Clone() { return (this.MemberwiseClone() as WarehouseCategory)!; }
}