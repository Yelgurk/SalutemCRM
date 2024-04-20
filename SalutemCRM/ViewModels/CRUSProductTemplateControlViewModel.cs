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

public partial class CRUSProductTemplateControlViewModelSource : ReactiveControlSource<ProductTemplate>
{
    [ObservableProperty]
    private ObservableCollection<ProductTemplate> _productTemplates = new() {
        new ProductTemplate() { Name = "Test 1" },
        new ProductTemplate() { Name = "Test 2" },
        new ProductTemplate() { Name = "Test 3" },
        new ProductTemplate() { Name = "Test 4" },
        new ProductTemplate() { Name = "Test 5" }
    };

    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
        {
            if (ProductCategory is null)
                ProductTemplates = new(
                from v in db.ProductTemplates
                    .Include(x => x.ProductSchemas)
                    .Include(x => x.Category)
                    .AsEnumerable()
                where keyword.Split(" ").Any(s => v.Name.ToLower().Contains(s)) select v);
            else
                ProductTemplates = new(
                from v in db.ProductTemplates
                    .Include(x => x.ProductSchemas)
                    .Include(x => x.Category)
                    .AsEnumerable()
                where keyword.Split(" ").Any(s => v.Name.ToLower().Contains(s)) &&
                      v.ManufactureCategoryForeignKey == ProductCategory.Id
                select v);
        }
    }

    [ObservableProperty]
    private ProductCategory? _productCategory = null;

    partial void OnProductCategoryChanged(ProductCategory? value) => SearchByInput(SearchInputStr);
}

public class CRUSProductTemplateControlViewModel : ViewModelBase<ProductTemplate, CRUSProductTemplateControlViewModelSource>
{
    public CRUSProductTemplateControlViewModel() : base(new() { PagesCount = 3 })
    {
        IfNewFilled = this.WhenAnyValue(
            x => x!.Source!.TempItem,
            x => x!.Source!.TempItem!.Name,
            x => x!.Source!.ProductCategory,
            (pt, pt_name, pt_cat) =>
                pt is not null &&
                pt_cat is not null &&
                !string.IsNullOrWhiteSpace(pt_name) &&
                pt_name.Length > 2
        );

        IfEditFilled = this.WhenAnyValue(
            x => x!.Source!.EditItem!.Name,
            x => x!.Source!.TempItem!.Name,
            x => x!.Source!.EditItem!.Category,
            x => x!.Source!.ProductCategory,
            (name_old, name_new, cat_old, cat_new) =>
                cat_new is not null &&
                !string.IsNullOrWhiteSpace(name_new) &&
                (name_old != name_new || (cat_old?.Name ?? "") != cat_new.Name) &&
                name_new.Length > 2
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
            Source!.TempItem = new();
            Source!.SetActivePage(1);
        });

        GoEditCommand = ReactiveCommand.Create<ProductTemplate>(x => {
            Source!
                .DoInst(s => s.EditItem = x.Clone())
                .DoInst(s => s.TempItem = s.EditItem!.Clone())
                .Do(s => s.SetActivePage(2));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source!.TempItem!
            .Do(x =>
            {
                using (DatabaseContext db = new(DatabaseContext.ConnectionInit())) db
                    .DoInst(y => x.ManufactureCategoryForeignKey = Source!.ProductCategory!.Id)
                    .DoInst(y => y.ProductTemplates.Add(x))
                    .Do(y => y.SaveChanges());
            })
            .DoInst(x => Source.SearchInputStr = x.Name)
            .Do(x => Source.SetActivePage(0))
            .Do(x => Source!.TempItem = new());
        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {
            Source!.TempItem!
            .DoInst(x => x.ManufactureCategoryForeignKey = Source!.ProductCategory!.Id)
            .Do(x => {
                using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                    db.ProductTemplates.Single(s => s.Id == x.Id)
                    .DoInst(y => y.Name = x.Name)
                    .DoInst(y => y.ManufactureCategoryForeignKey = x.ManufactureCategoryForeignKey)
                    .DoInst(y => y.AdditionalInfo = x.AdditionalInfo)
                    .Do(y => db.SaveChanges());
            })
            .DoInst(x => Source!.SearchInputStr = x.Name)
            .Do(x => Source.SetActivePage(0))
            .Do(x => Source.EditItem = new())
            .Do(x => Source.TempItem = new());
        }, IfEditFilled);
            

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source!.SearchByInput("");

        Source!.SetActivePage(0);
    }
}
