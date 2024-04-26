using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
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
    [ObservableProperty]
    private WarehouseSupply _newOrderWarehouseSupplyInput = new() { WarehouseItem = new() };

    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _orderWarehouseSupplies = new();

    [ObservableProperty]
    private ObservableCollection<string> _mesurementUnits = new();

    [ObservableProperty]
    private ObservableCollection<string> _currencyUnits = new();

    partial void OnNewOrderWarehouseSupplyInputChanged(WarehouseSupply value) => value.WarehouseItem = new();

    public void CollectionReIndex() => 0.Do(x => OrderWarehouseSupplies.DoForEach(y => y.Id = ++x));
}

public partial class OrderEditorControlViewModel : ViewModelBase<Order, OrderEditorControlViewModelSource>
{
    public IObservable<bool>? IfNewItemFilled { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddNewOrderItem { get; protected set; }

    public ReactiveCommand<WarehouseSupply, Unit>? RemoveNewOrderItem { get; protected set; }

    public OrderEditorControlViewModel() : base(new() { PagesCount = 1 })
    {
        if (!Design.IsDesignMode)
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit())) db
                .DoInst(x => Source.MesurementUnits = new(db.MesurementUnits.Select(y => y.Name)))
                .DoInst(x => Source.CurrencyUnits = new(db.CurrencyUnits.Select(y => y.Name)));

        IfNewItemFilled = this.WhenAnyValue(
            x => x.Source!.NewOrderWarehouseSupplyInput.WarehouseItem.InnerName,
            x => x.Source!.NewOrderWarehouseSupplyInput.Currency,
            x => x.Source!.NewOrderWarehouseSupplyInput.OrderCount,
            x => x.Source!.NewOrderWarehouseSupplyInput.OrderPriceSingleBYN,
            (name, curr_u, count, price) =>
                !string.IsNullOrWhiteSpace(name) &&
                !string.IsNullOrWhiteSpace(curr_u) &&
                name.Length > 0 &&
                curr_u.Length > 0 &&
                count > 0.0 &&
                price > 0.0
        );

        AddNewOrderItem = ReactiveCommand.Create(() => {
            App.Host!.Services.GetService<TCPChannel>()!.Send(Source.NewOrderWarehouseSupplyInput.WarehouseItem.InnerName);

            Source.OrderWarehouseSupplies.Add(Source.NewOrderWarehouseSupplyInput);
            Source.NewOrderWarehouseSupplyInput
                .DoInst(x => Source.NewOrderWarehouseSupplyInput = new() {
                    WarehouseItem = new(),
                    Currency = x.Currency,
                    UnitToBYNConversion = x.UnitToBYNConversion
                });
            Source.CollectionReIndex();
        }, IfNewItemFilled);

        RemoveNewOrderItem = ReactiveCommand.Create<WarehouseSupply>(x => {
            Source.OrderWarehouseSupplies.Remove(x);
            Source.CollectionReIndex();
        });
    }
}
