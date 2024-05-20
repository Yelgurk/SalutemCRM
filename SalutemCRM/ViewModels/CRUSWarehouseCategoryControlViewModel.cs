using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Avalonia.ReactiveUI;
using DynamicData;
using System.Threading;
using Avalonia.Threading;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.Services;

namespace SalutemCRM.ViewModels;

public partial class CRUSWarehouseCategoryControlViewModelSource : ReactiveControlSource<WarehouseCategory>
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CategoriesTree))]
    private ObservableCollection<WarehouseCategory> _warehouseCategories = new() {
        new WarehouseCategory() { Name = "Test 1", Deep = 0 },
        new WarehouseCategory() { Name = "Test 2", Deep = 0 },
        new WarehouseCategory() { Name = "Test 3", Deep = 0 },
        new WarehouseCategory() { Name = "Test 4", Deep = 0 },
        new WarehouseCategory() { Name = "Test 5", Deep = 0 }
    };

    public HierarchicalTreeDataGridSource<WarehouseCategory> CategoriesTree => new(WarehouseCategories)
    {
        Columns =
        {
            new HierarchicalExpanderColumn<WarehouseCategory>(
                new TextColumn<WarehouseCategory, string> ("Name", x => $"{x.Name}"), x => x.SubCategories
                ),
            //new TextColumn<WarehouseCategory, string> ("Name", x => x.Name)
        }
    };

    [ObservableProperty]
    private bool _isSearchMatch = false;

    public override void SearchByInput(string keyword)
    {
        IsSearchMatch = false;
        SelectedItem = null;
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
        {
            int MaxDeep = db.WarehouseCategories.Max(wc => wc.Deep);
            var CatTree = db.WarehouseCategories.Where(wc => wc.Deep == 0).ToList();
            List<WarehouseCategory> Cat = new();
            List<WarehouseCategory> CatMatch = new();

            for (var CatDeep = CatTree; (CatDeep.FirstOrDefault()?.Deep ?? MaxDeep) != MaxDeep;)
                CatDeep = CatDeep
                    .DoForEach(x => x.SubCategories = new(db.WarehouseCategories.Where(wc => wc.ParentCategoryForeignKey == x.Id)))
                    .DoForEach(x => x.SubCategories.DoForEach(c => c.ParentCategory = x))
                    .DoInst(x => x.DoIf(y => Cat.Add(y), y => y.First().Deep == 0))
                    .Do(x => x.SelectMany(sm => sm.SubCategories))
                    .Do(x => Cat.Add(x))
                    .Do(x => x.ToList());

            WarehouseCategories = new(CatTree);
        }
    }
}

public class CRUSWarehouseCategoryControlViewModel : ViewModelBase<WarehouseCategory, CRUSWarehouseCategoryControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? RemoveCategoryInheritanceCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? UnselectCommand { get; protected set; }

    public CRUSWarehouseCategoryControlViewModel() : base(new() { PagesCount = 3 })
    {
        UnselectCommand = ReactiveCommand.Create(() => { Source.SelectedItem = null; }, CRUSWarehouseCategoryControlViewModelSource.GlobalContainer.IsSelectedItemNotNull);

        IfNewFilled = this.WhenAnyValue(
            x => x.Source.TempItem,
            x => x.Source.TempItem!.Name,
            (obj, name) =>
                obj != null &&
                !string.IsNullOrWhiteSpace(name) &&
                name.Length >= 2
        );

        IfEditFilled = this.WhenAnyValue(
            x => x.Source.EditItem!.Name,
            x => x.Source.TempItem!.Name,
            (old_name, new_name) =>
                !string.IsNullOrWhiteSpace(new_name) &&
                new_name.Length >= 2 &&
                old_name != new_name
        );

        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(() => {
            Source.SelectedItem = null;
            Source.SetActivePage(0);
        });

        GoAddCommand = ReactiveCommand.Create(() => {
            Source
                .Do(s => s.SearchByInput(""))
                .DoInst(x => x.TempItem = new())
                .Do(x => x.SetActivePage(1));
        });

        GoEditCommand = ReactiveCommand.Create<WarehouseCategory>(x => {
            Source
                .DoInst(s => s.EditItem = x.Clone())
                .DoInst(s => s.TempItem = s.EditItem!.Clone())
                .DoInst(s => s.SelectedItem = null)
                .Do(s => s.SearchByInput(""))
                .DoIf(s => {
                    using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                        s.SelectedItem = s.TempItem!.ParentCategory
                        .Do(pc => pc = db.WarehouseCategories.SingleOrDefault(z => z.Id == s.TempItem!.ParentCategory!.Id));
                }, s => s.TempItem!.ParentCategory is not null)
                .Do(s => Source.SetActivePage(2));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                    x.TempItem
                    .DoInst(w => w!.ParentCategory = x.SelectedItem is null ? null : db.WarehouseCategories.Single(s => s.Id == x.SelectedItem.Id))
                    .DoInst(w => w!.Deep = w.ParentCategory is null ? 0 : w.ParentCategory.Deep + 1)
                    .Do(w => db.WarehouseCategories.Add(w!))
                    .Do(w => db.SaveChanges());
            }, x => x.TempItem != null)?
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .DoInst(x => x.TempItem = new())
            .Do(x => x.SetActivePage(0));
        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                    db.WarehouseCategories.Single(s => s.Id == x.EditItem!.Id)
                    .DoIf(e => { }, e => x.SelectedItem is null || x.SelectedItem.Id != e.Id)?
                    .DoInst(e => e.Name = x.TempItem!.Name)
                    .DoInst(e => e.ParentCategoryForeignKey = x.SelectedItem is null ? null : db.WarehouseCategories.Single(s => s.Id == x.SelectedItem!.Id).Id)
                    .DoInst(e => e.Deep = x.SelectedItem?.Deep + 1 ?? 0)
                    .DoInst(e => db.SaveChanges());
            }, x => x.TempItem != null)?
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .DoInst(x => x.TempItem = new())
            .Do(x => x.SetActivePage(0));
        }, IfEditFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);

        RemoveCategoryInheritanceCommand = ReactiveCommand.Create(() => {
            Source.SelectedItem = null;
        });



        if (!Design.IsDesignMode)
            Source.SearchByInput("");

        Source.SetActivePage(0);
    }
}
