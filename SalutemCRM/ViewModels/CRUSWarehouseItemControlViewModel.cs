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

public partial class CRUSWarehouseItemControlViewModelSource : ReactiveControlSource<WarehouseItem>
{
    public const int CodeTemplate = 1000000;

    [ObservableProperty]
    private string _innerCodeNew = "";



    [ObservableProperty]
    private ObservableCollection<WarehouseItem> _warehouseItems = new() {
        new WarehouseItem() { InnerName = "Test 1" },
        new WarehouseItem() { InnerName = "Test 2" },
        new WarehouseItem() { InnerName = "Test 3" },
        new WarehouseItem() { InnerName = "Test 4" },
        new WarehouseItem() { InnerName = "Test 5" }
    };

    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
        {
            if (WarehouseCategory is null)
                WarehouseItems = new(
                from v in db.WarehouseItems
                    .Include(x => x.Category)
                    .Include(x => x.WarehouseSupplying)
                    .AsEnumerable()
                where keyword.Split(" ").Any(s =>
                    v.InnerName.ToLower().Contains(s) ||
                    v.InnerCode.ToLower().Contains(s)
                )
                select v
                );
            else
                WarehouseItems = new(
                from v in db.WarehouseItems
                    .Include(x => x.Category)
                    .Include(x => x.WarehouseSupplying)
                    .AsEnumerable()
                where keyword.Split(" ").Any(s =>
                    v.InnerName.ToLower().Contains(s) ||
                    v.InnerCode.ToLower().Contains(s)
                ) && v.WarehouseCategoryForeignKey == WarehouseCategory.Id
                select v
                );
        }

        WarehouseItems.DoForEach(x => x.WarehouseSupplying.Do(ws => ws.RemoveMany(ws.Where(s => s.InStockCount == 0.0))));
    }

    [ObservableProperty]
    private WarehouseCategory? _warehouseCategory = null;

    partial void OnWarehouseCategoryChanged(WarehouseCategory? value) => SearchByInput(SearchInputStr);
}

public class CRUSWarehouseItemControlViewModel : ViewModelBase<WarehouseItem, CRUSWarehouseItemControlViewModelSource>
{
    public CRUSWarehouseItemControlViewModel() : base(new() { PagesCount = 3 })
    {
        IfNewFilled = this.WhenAnyValue(
            x => x.Source.TempItem,
            x => x.Source.TempItem!.InnerName,
            x => x.Source.WarehouseCategory,
            x => x.Source.InnerCodeNew,
            x => x.Source.TempItem!.MesurementUnit,
            (obj, name, cat, code, mu) =>
                obj != null &&
                cat != null &&
                !string.IsNullOrWhiteSpace(name) &&
                !string.IsNullOrWhiteSpace(code) &&
                !string.IsNullOrWhiteSpace(mu) &&
                name.Length >= 2 && code.Length > 0 && mu.Length > 0
        );

        IfEditFilled = this.WhenAnyValue(
            x => x.Source.EditItem!.InnerName,
            x => x.Source.TempItem!.InnerName,
            x => x.Source.TempItem!.Category,
            x => x.Source.WarehouseCategory,
            x => x.Source.EditItem!.MesurementUnit,
            x => x.Source.EditItem!.CountRequired,
            x => x.Source.TempItem!.MesurementUnit,
            x => x.Source.TempItem!.CountRequired,
            (old_name, new_name, cat_old, cat_new, mu_old, cr_old, mu_new, cr_new) =>
                cat_new is not null &&
                cat_old is not null &&
                !string.IsNullOrWhiteSpace(new_name) &&
                !string.IsNullOrWhiteSpace(mu_new) &&
                new_name.Length >= 2 &&
                (old_name != new_name ||
                 cat_old.Name != cat_new.Name ||
                 mu_old != mu_new ||
                 cr_old != cr_new)
        );

        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(() => {
            Source!.SetActivePage(0);
        });

        GoAddCommand = ReactiveCommand.Create(() => {
            if (!Design.IsDesignMode)
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                Source!.InnerCodeNew = $"{db.WarehouseItems.Count() + 1 + CRUSWarehouseItemControlViewModelSource.CodeTemplate}";

            Source!.TempItem = new();
            Source!.SetActivePage(1);
        });

        GoEditCommand = ReactiveCommand.Create<WarehouseItem>(x => {
            Source!
                .DoInst(s => s.EditItem = x.Clone())
                .DoInst(s => s.TempItem = s.EditItem!.Clone())
                .Do(s => s.SetActivePage(2));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source!.TempItem?
                .DoInst(x => x.InnerCode = Source.InnerCodeNew)
                .DoInst(x => x.WarehouseCategoryForeignKey = Source.WarehouseCategory!.Id)
                .Do(x =>
                {
                    using (DatabaseContext db = new(DatabaseContext.ConnectionInit())) db
                        .DoInst(y => x.Category = y.WarehouseCategories.Single(s => s.Id == Source.WarehouseCategory!.Id))
                        .DoInst(y => y.WarehouseItems.Add(x))
                        .Do(y => y.SaveChanges());
                })
                .Do(x => Source.SearchInputStr = x.InnerName)
                .Do(x => Source.SetActivePage(0))
                .Do(x => Source.TempItem = new());
        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                db.WarehouseItems.Single(x => x.Id == Source!.EditItem!.Id)
                .DoInst(x => x.InnerName = Source!.TempItem!.InnerName)
                .DoInst(x => x.MesurementUnit = Source!.TempItem!.MesurementUnit)
                .DoInst(x => x.CountRequired = Source!.TempItem!.CountRequired)
                .DoInst(x => x.WarehouseCategoryForeignKey = Source!.WarehouseCategory!.Id)
                .DoInst(x => db.SaveChanges())
                .Do(x => Source!.SearchInputStr = x.InnerName)
                .Do(x => Source!.SetActivePage(0))
                .Do(x => Source!.TempItem = new())
                .Do(x => Source!.EditItem = new());
        }, IfEditFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source!.SearchByInput("");

        Source!.SetActivePage(0);
    }
}
