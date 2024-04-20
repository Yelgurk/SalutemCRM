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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SalutemCRM.ViewModels;

public partial class ProductSchemaBuilderViewModelSource : ReactiveControlSource<ProductSchema>
{
    [ObservableProperty]
    private ObservableCollection<ProductSchema>? _productSchemas = new() {
        new ProductSchema() { Count = 1 },
        new ProductSchema() { Count = 2 },
        new ProductSchema() { Count = 3 },
        new ProductSchema() { Count = 4 },
        new ProductSchema() { Count = 5 }
    };

    public override void SearchByInput(string keyword)
    {
        if (ProductTemplate is not null)
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                ProductSchemas = new(
                    db.ProductSchemas
                    .Where(x => x.ProductTemplateForeignKey == ProductTemplate.Id)
                    .Include(x => x.ProductTemplate)
                    .Include(x => x.WarehouseItem)
                        .ThenInclude(x => x!.Category)
                );
        else
            ProductSchemas = null;
    }

    [ObservableProperty]
    private WarehouseItem? _warehouseItem = null;

    [ObservableProperty]
    private ProductTemplate? _productTemplate = null;

    partial void OnProductTemplateChanged(ProductTemplate? oldValue, ProductTemplate? newValue) =>
        (oldValue, newValue).DoIf(x => SearchByInput(""), x => (x.oldValue?.Name ?? "") != (x.newValue?.Name ?? ""));

    public bool AcceptTemplateChanges()
    {
        if (ProductSchemas is null || ProductTemplate is null)
            return false;

        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            (from ps in db.ProductSchemas where !ProductSchemas.Select(x => x.Id).Contains(ps.Id) && ps.ProductTemplateForeignKey == ProductTemplate.Id select ps)
            .DoForEach(x => db.ProductSchemas.Remove(x))
            .Do(x => db.SaveChanges())

            .Do(x => from ps in db.ProductSchemas where ProductSchemas.Select(x => x.Id).Contains(ps.Id) && ps.ProductTemplateForeignKey == ProductTemplate.Id select ps)
            .DoForEach(x => x.Count = ProductSchemas.Single(ps => ps.Id == x.Id).Count)
            .DoForEach(x => ProductSchemas.Remove(ProductSchemas.Single(y => y.Id == x.Id)))
            .Do(x => db.SaveChanges())

            .Do(x => ProductSchemas)
            .DoForEach(x => db.ProductSchemas.Add(new() {
                ProductTemplateForeignKey = x.ProductTemplateForeignKey,
                WarehouseItemForeignKey = x.WarehouseItemForeignKey,
                Count = x.Count
            }))
            .Do(x => db.SaveChanges());

        SearchByInput("");

        return true;
    }

    public bool CancelTemplateChanges()
    {
        if (ProductSchemas is null)
            return false;

        this.SearchByInput("");

        return true;
    }
}

public class ProductSchemaBuilderViewModel : ViewModelBase<ProductSchema, ProductSchemaBuilderViewModelSource>
{
    public IObservable<bool>? IfItemSelected { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddSelectedItemCommand { get; protected set; }

    public ReactiveCommand<ProductSchema, Unit>? RemoveItemCommand { get; protected set; }
    
    public ReactiveCommand<Unit, Unit>? AcceptProductTemplateCommand { get; protected set; }

    public ProductSchemaBuilderViewModel() : base(new() { PagesCount = 3 })
    {
        IfItemSelected = this.WhenAnyValue(
            x => x.Source!.WarehouseItem,
            x => x.Source!.ProductTemplate,
            (wi, pt) =>
                wi is not null &&
                pt is not null
        );

        AddSelectedItemCommand = ReactiveCommand.Create(() => {
            Source!.ProductSchemas!
                .DoIf(x => x.Add(new()
                {
                    WarehouseItem = Source!.WarehouseItem,
                    WarehouseItemForeignKey = Source!.WarehouseItem!.Id,
                    ProductTemplate = Source!.ProductTemplate,
                    ProductTemplateForeignKey = Source.ProductTemplate!.Id,
                    Count = 0
                }), x => x.SingleOrDefault(y => y.WarehouseItemForeignKey == Source!.WarehouseItem!.Id) is null);

            Source!.ProductSchemas!.Single(x => x.WarehouseItemForeignKey == Source.WarehouseItem!.Id).Count += 1.0;
        }, IfItemSelected);

        RemoveItemCommand = ReactiveCommand.Create<ProductSchema>(x => {
            Source!.ProductSchemas!.Remove(x);
        });

        AcceptProductTemplateCommand = ReactiveCommand.Create(() => {
            Source!
            .DoInst(x => x.AcceptTemplateChanges())
            .Do(x => x.SearchByInput(""));
        });

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source!.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source!.SearchByInput("");

        Source!.SetActivePage(0);
    }
}
