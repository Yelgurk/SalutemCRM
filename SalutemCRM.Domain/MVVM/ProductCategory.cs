using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain.Model;

public partial class ProductCategory
{
    public ProductCategory() { }

    public ProductCategory(ProductCategory source)
    {
        this.Id = source.Id;
        this.Name = source.Name;
        this.Deep = source.Deep;
    }

    [NotMapped]
    public string NameHierarchyToZeroDeepParent => $"{this.Name} {ParentCategory?.Name ?? ""}";

    [NotMapped]
    public List<ProductCategory> ObjHierarchyToZeroDeepParent =>
        new List<ProductCategory>()
        .Do(x => x.Add(this))
        .DoInst(x => x.DoIf(y => y.AddRange(this.ParentCategory!.Clone().Do(z => { z.SubCategories.Clear(); z.SubCategories.Add(this); }).ObjHierarchyToZeroDeepParent), y => this.ParentCategory is not null));

    public void ObjHierarchyToLastChild(List<ProductCategory> list)
    {
        list
            .DoIf(x => this.SubCategories.Add(new(x.First())), x => x.Count > 0)?
            .Do(x => x.RemoveAt(0))
            .DoIf(x => this.SubCategories.First().ObjHierarchyToLastChild(x), x => x.Count > 0);
    }
}