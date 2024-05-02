﻿using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using SalutemCRM.TCP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class OrderEditorControlViewModelSource : ReactiveControlSource<Order>
{
    public OrderEditorControlViewModelSource() => SelectedItem = Order.Default;

    /* Warehouse supplying */
    [ObservableProperty]
    private WarehouseSupply _newOrderWarehouseSupplyInput = new();

    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _orderWarehouseSupplies = new();


    /* Manager product sale */
    [ObservableProperty]
    private ObservableCollection<ProductTemplate> _orderManagerProduct = new();


    /* Mesurement unit */
    [ObservableProperty]
    private ObservableCollection<string> _mesurementUnits = new();

    [ObservableProperty]
    private ObservableCollection<string> _currencyUnits = new();


    public void CollectionReIndex() => 0.Do(x => OrderWarehouseSupplies.DoForEach(y => y.Id = ++x));
}

public partial class OrderEditorControlViewModel : ViewModelBase<Order, OrderEditorControlViewModelSource>
{
    public IObservable<bool>? IfNewItemFilled { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddNew_WarehouseSupply { get; protected set; }
    public ReactiveCommand<Unit, Unit>? AddNew_ManagerProductSale { get; protected set; }

    public ReactiveCommand<WarehouseSupply, Unit>? RemoveNew_WarehouseSupply { get; protected set; }
    public ReactiveCommand<ProductTemplate, Unit>? RemoveNew_ManagerProductSale { get; protected set; }

    public OrderEditorControlViewModel() : base(new() { PagesCount = 1 })
    {
        if (!Design.IsDesignMode)
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit())) db
                .DoInst(x => Source.MesurementUnits = new(db.MesurementUnits.Select(y => y.Name)))
                .DoInst(x => Source.CurrencyUnits = new(db.CurrencyUnits.Select(y => y.Name)));

        IfNewItemFilled = this.WhenAnyValue(
            x => x.Source!.NewOrderWarehouseSupplyInput.VendorName,
            x => x.Source!.NewOrderWarehouseSupplyInput.Currency,
            x => x.Source!.NewOrderWarehouseSupplyInput.OrderBuilder_Count,
            x => x.Source!.NewOrderWarehouseSupplyInput.OrderBuilder_PriceSingle,
            (name, curr_u, count, price) =>
                !string.IsNullOrWhiteSpace(name) &&
                !string.IsNullOrWhiteSpace(curr_u) &&
                name.Length > 0 &&
                curr_u.Length > 0 &&
                count > 0.0 &&
                price > 0.0
        );

        AddNew_WarehouseSupply = ReactiveCommand.Create(() => {
            Source.OrderWarehouseSupplies.Add(Source.NewOrderWarehouseSupplyInput);
            Source.NewOrderWarehouseSupplyInput
                .DoInst(x => Source.NewOrderWarehouseSupplyInput = new() {
                    Currency = x.Currency,
                    UnitToBYNConversion = x.UnitToBYNConversion
                });
            Source.CollectionReIndex();
        }, IfNewItemFilled);

        AddNew_ManagerProductSale = ReactiveCommand.Create(() => {
            CRUSProductTemplateControlViewModelSource.GlobalContainer.SelectedItem!
            .Do(gsi =>
            {
                var x = gsi.Clone();
                x.Category = gsi.Category;
                return x;
            })
            .Do(x =>
            {
                try
                {
                    if (Source.OrderManagerProduct.Single(s => s.Name == x.Name && s.Category!.Name == x.Category!.Name) is var match && !match.HaveSerialNumber)
                        ++match.OrderBasketCount;
                    else
                        throw new Exception();
                }
                catch { Source.OrderManagerProduct.Add(x); }
            });
        }, CRUSProductTemplateControlViewModelSource.GlobalContainer.IsSelectedItemNotNull);

        RemoveNew_WarehouseSupply = ReactiveCommand.Create<WarehouseSupply>(x => {
            Source.OrderWarehouseSupplies.Remove(x);
            Source.CollectionReIndex();
        });

        RemoveNew_ManagerProductSale = ReactiveCommand.Create<ProductTemplate>(x => {
            Source.OrderManagerProduct.Remove(x);
        });
    }
}
