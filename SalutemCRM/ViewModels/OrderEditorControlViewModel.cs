using Avalonia.Controls;
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
        IfNewItemFilled = this.WhenAnyValue(
            x => x.Source!.NewOrderWarehouseSupplyInput.WarehouseItem.InnerName,
            x => x.Source!.NewOrderWarehouseSupplyInput.OrderCount,
            x => x.Source!.NewOrderWarehouseSupplyInput.OrderPriceSingleBYN,
            (name, count, price) =>
                !string.IsNullOrWhiteSpace(name) &&
                name.Length > 0 &&
                count > 0.0 &&
                price > 0.0
        );

        AddNewOrderItem = ReactiveCommand.Create(() => {
            Source.OrderWarehouseSupplies.Add(Source.NewOrderWarehouseSupplyInput);
            Source.NewOrderWarehouseSupplyInput = new() { WarehouseItem = new() };
            Source.CollectionReIndex();
        }, IfNewItemFilled);

        RemoveNewOrderItem = ReactiveCommand.Create<WarehouseSupply>(x => {
            Source.OrderWarehouseSupplies.Remove(x);
            Source.CollectionReIndex();
        });
    }
}
