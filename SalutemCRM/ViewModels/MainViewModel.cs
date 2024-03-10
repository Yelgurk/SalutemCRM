using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls.Documents;
using Avalonia.Threading;

namespace SalutemCRM.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public HierarchicalTreeDataGridSource<WarehouseCategory> CategoriesTree { get; set; }

    public MainViewModel()
    {
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
        {
            WarehouseCategory Last = null!;

            int MaxDeep = db.WarehouseCategories.Max(wc => wc.Deep);
            var CatTree = db.WarehouseCategories.Where(wc => wc.Deep == 0).ToList();
            for (var CatDeep = CatTree; CatDeep.First().Deep != MaxDeep;)
            {
                foreach (var Cat in CatDeep)
                    Cat.SubCategories = new ObservableCollection<WarehouseCategory>(db.WarehouseCategories.Where(wc => wc.ParentCategoryForeignKey == Cat.Id).ToList());

                CatDeep = CatDeep.SelectMany(ct => ct.SubCategories).ToList();
                Last = CatDeep.Last();
            }

            CategoriesTree = new HierarchicalTreeDataGridSource<WarehouseCategory>(CatTree)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<WarehouseCategory>(
                        new TextColumn<WarehouseCategory, string>
                            ("Id", x => $"{x.Id}."), x => x.SubCategories),
                        new TextColumn<WarehouseCategory, string>
                            ("Name", x => x.Name),
                },
            };
        }
    }
}
