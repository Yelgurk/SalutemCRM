using Avalonia.Controls;
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


    /* Service warehouse item sale */
    [ObservableProperty]
    private ObservableCollection<WarehouseItem> _orderServiceItem = new();


    /* Mesurement unit */
    [ObservableProperty]
    private ObservableCollection<string> _mesurementUnits = new();

    [ObservableProperty]
    private ObservableCollection<string> _currencyUnits = new();


    public void OrderTotalPriceCalc() => SelectedItem!.PriceRequired = 0.5 * (SelectedItem!.PriceTotal = OrderWarehouseSupplies.Select(x => x.OrderBuilder_PriceTotalBYN).Sum());

    public void CollectionReIndex() => 0.Do(x => OrderWarehouseSupplies.DoForEach(y => y.Id = ++x)).Do(x => OrderTotalPriceCalc());

    public void ClearOrderBuilder()
    {
        SelectedItem = Order.Default;
        NewOrderWarehouseSupplyInput = new();
        OrderWarehouseSupplies.Clear();
        OrderManagerProduct.Clear();
        OrderServiceItem.Clear();
    }

    public void CreateNewOrderBasedOnBuilder()
    {
        DateTime RecordDT = DateTime.Now;

        new Order()
            .Do(order =>
            {
                order.EmployeeForeignkey = Account.Current.User.Id;

                order.OrderType = SelectedItem!.OrderType;
                order.PaymentAgreement = SelectedItem!.PaymentAgreement;

                order.PaymentStatus = order.PaymentAgreement == Payment_Status.Unpaid ? Payment_Status.FullyPaid : Payment_Status.Unpaid;
                order.TaskStatus = order.OrderType == Order_Type.CustomerService ? Task_Status.NotAvailable : Task_Status.AwaitPayment;

                order.AdditionalInfo = string.IsNullOrEmpty(SelectedItem!.AdditionalInfo) ? $"[{Account.Current.User.FullNameWithLogin} : нет дополнительной информации]" : SelectedItem!.AdditionalInfo;
                order.DaysOnHold = SelectedItem!.DaysOnHold;
                order.Currency = SelectedItem!.Currency;
                order.UnitToBYNConversion = SelectedItem!.UnitToBYNConversion < 0 ? 1.00 : SelectedItem!.UnitToBYNConversion;
                order.PriceRequired = SelectedItem!.PaymentAgreement switch
                {
                    Payment_Status.Unpaid => 0,
                    Payment_Status.PartiallyPaid => SelectedItem!.PriceRequired,
                    Payment_Status.FullyPaid => SelectedItem!.PriceTotal,
                    _ => -1
                };
                order.PriceTotal = SelectedItem!.PriceTotal;
                order.RecordDT = RecordDT;
                order.DeadlineDT = null;
                order.StartedDT = null;
                order.CompletedDT = null;

                if (order.IsCustomerOrder && CRUSClientControlViewModelSource.GlobalContainer.SelectedItem == null)
                    return null;

                if (!order.IsCustomerOrder && CRUSVendorControlViewModelSource.GlobalContainer.SelectedItem == null)
                    return null;

                return order.OrderType switch
                {
                    Order_Type.CustomerService => order.DoInst(x => x.ClientForeignKey = CRUSClientControlViewModelSource.GlobalContainer.SelectedItem!.Id),

                    Order_Type.ManagerSale =>
                        OrderManagerProduct.Count == 0 ?
                        null :
                        order.DoInst(x => x.ClientForeignKey = CRUSClientControlViewModelSource.GlobalContainer.SelectedItem!.Id),

                    Order_Type.WarehouseRestocking =>
                        OrderWarehouseSupplies.Count == 0 ?
                        null :
                        order.DoInst(x => x.VendorForeignKey = CRUSVendorControlViewModelSource.GlobalContainer.SelectedItem!.Id),

                    _ => null
                };
            })?
            .Do(x =>
            {
                int OrderID = 0;

                using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                {
                    db.Orders.Add(x);
                    Debug.WriteLine(x.TaskStatus);
                    db.SaveChanges();

                    OrderID = db.Orders.Single(s =>
                            s.RecordDT == RecordDT &&
                            s.EmployeeForeignkey == x.EmployeeForeignkey &&
                            (s.ClientForeignKey == x.ClientForeignKey || s.VendorForeignKey == x.VendorForeignKey)
                        ).Id;

                    if (x.OrderType == Order_Type.CustomerService)
                    {
                        foreach (var item in OrderServiceItem)
                            db.MaterialFlow.Add(new()
                            {
                                ReturnedForeignKey = null,
                                EmployeeForeignKey = x.EmployeeForeignkey,
                                WarehouseItemForeignKey = item.Id,
                                WarehouseSupplyForeignKey = null,
                                ManufactureForeignKey = null,
                                OrderForeignKey = OrderID,
                                AdditionalInfo = x.AdditionalInfo ?? $"[счёт #{OrderID}, {Account.Current.User.FullNameWithLogin} : нет дополнительной информации]",
                                CountReservedFromStock = item.OrderBuilder_Count,
                                CountProvidedFromStock = 0,
                                CountReturnedToStock = 0,
                                DeliveryStatus = Delivery_Status.NotDelivered,
                                RecordDT = RecordDT
                            });
                    }
                    else if (x.OrderType == Order_Type.ManagerSale)
                    {
                        foreach (var prod in OrderManagerProduct)
                            db.Manufacture.Add(new()
                            {
                                OrderForeignKey = OrderID,
                                DeliveryStatus = Delivery_Status.NotDelivered,
                                TaskStatus = Task_Status.NotAvailable,
                                HaveSerialNumber = prod.HaveSerialNumber,
                                Name = prod.Name,
                                Code = null,
                                Count = prod.HaveSerialNumber ? 1 : prod.OrderBasketCount,
                                AdditionalInfo = prod.AdditionalInfo ?? $"[счёт #{OrderID}, {Account.Current.User.FullNameWithLogin} : нет дополнительной информации]",
                                ShipmentDT = null
                            });
                    }
                    else if (x.OrderType == Order_Type.WarehouseRestocking)
                    {
                        foreach (var item in OrderWarehouseSupplies)
                            db.WarehouseSupplying.Add(new()
                            {
                                WarehouseItemForeignKey = null,
                                OrderForeignKey = OrderID,
                                DeliveryStatus = Delivery_Status.NotDelivered,
                                VendorName = item.VendorName,
                                VendorCode = null,
                                Currency = item.Currency,
                                UnitToBYNConversion = item.OrderBuilder_ToBYNConv,
                                PriceTotal = item.OrderBuilder_PriceTotal,
                                OrderCount = item.OrderBuilder_Count,
                                InStockCount = 0,
                                RecordDT = RecordDT
                            });
                    }

                    db.SaveChanges();
                }

                FileSelectorControlViewModelSource.AttachFilesTo(x);
                ClearOrderBuilder();
            });
    }
}

public partial class OrderEditorControlViewModel : ViewModelBase<Order, OrderEditorControlViewModelSource>
{
    public IObservable<bool>? IfNewItemFilled { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddNew_WarehouseSupply { get; protected set; }
    public ReactiveCommand<Unit, Unit>? AddNew_ManagerProductSale { get; protected set; }
    public ReactiveCommand<Unit, Unit>? AddNew_ServiceItemSale { get; protected set; }

    public ReactiveCommand<WarehouseSupply, Unit>? RemoveNew_WarehouseSupply { get; protected set; }
    public ReactiveCommand<ProductTemplate, Unit>? RemoveNew_ManagerProductSale { get; protected set; }
    public ReactiveCommand<WarehouseItem, Unit>? RemoveNew_ServiceItemSale { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AcceptNewOrder { get; protected set; }
    public ReactiveCommand<Unit, Unit>? ClearNewOrder { get; protected set; }

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

        AddNew_ServiceItemSale = ReactiveCommand.Create(() => {
            CRUSWarehouseItemControlViewModelSource.GlobalContainer.SelectedItem!
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
                    if (Source.OrderServiceItem.Single(s => s.InnerName == x.InnerName && s.InnerCode == x.InnerCode) is var match)
                        ++match.OrderBuilder_Count;
                    else
                        throw new Exception();
                }
                catch { Source.OrderServiceItem.Add(x); }
            });
        }, CRUSWarehouseItemControlViewModelSource.GlobalContainer.IsSelectedItemNotNull);

        RemoveNew_WarehouseSupply = ReactiveCommand.Create<WarehouseSupply>(x => {
            Source.OrderWarehouseSupplies.Remove(x);
            Source.CollectionReIndex();
        });

        RemoveNew_ManagerProductSale = ReactiveCommand.Create<ProductTemplate>(x => {
            Source.OrderManagerProduct.Remove(x);
        });

        RemoveNew_ServiceItemSale = ReactiveCommand.Create<WarehouseItem>(x => {
            Source.OrderServiceItem.Remove(x);
        });

        AcceptNewOrder = ReactiveCommand.Create(Source.CreateNewOrderBasedOnBuilder);

        ClearNewOrder = ReactiveCommand.Create(Source.ClearOrderBuilder);
    }
}
