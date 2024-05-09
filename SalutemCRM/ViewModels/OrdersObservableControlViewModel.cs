using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SalutemCRM.ViewModels;

public partial class OrdersObservableControlViewModelSource : ReactiveControlSource<Order>
{
    [ObservableProperty]
    private ObservableCollection<Order> _ordersCollection = new();

    [ObservableProperty]
    private double _bookkeeperPrice = 0;

    [ObservableProperty]
    private double _unitToBYNConv = 1.0;

    [ObservableProperty]
    private ObservableCollection<string> _currencyUnits = new();

    [ObservableProperty]
    private string _currency = "BYN";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentSelector))]
    private bool _isOnlyFullyPaid = false;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentSelector))]
    private bool _isOnlyNotPaid = true;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentSelector))]
    private bool _isOnlyPartialPaid = true;

    [ObservableProperty]
    private DateTime _dtSortBegin = DateTime.Today;

    [ObservableProperty]
    private DateTime _dtSortEnd = DateTime.Today.AddDays(1);

    public List<Payment_Status> PaymentSelector => new List<Payment_Status>()
        .Do(x => { if (IsOnlyFullyPaid) x.Add(Payment_Status.FullyPaid); })
        .Do(x => { if (IsOnlyNotPaid) x.Add(Payment_Status.Unpaid); })
        .Do(x => { if (IsOnlyPartialPaid) x.Add(Payment_Status.PartiallyPaid); });

    public void UpdateOrdersList()
    {
        Order? temp = SelectedItem;

        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
        {
            OrdersCollection = new(
                from item in db.Orders
                    .Include(x => x.Employee)
                    .Include(x => x.Client)
                    .Include(x => x.Vendor)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.FileAttachs)
                where PaymentSelector.Any(s => item.PaymentStatus == s) &&
                    item.RecordDT >= DtSortBegin && item.RecordDT <= DtSortEnd
                select item
            );
        }

        if (temp is not null)
            SelectedItem = OrdersCollection.SingleOrDefault(x => x.Id == temp.Id);
    }

    public void PaymentAccept()
    {
        DateTime RecordDT = DateTime.Now;

        if (SelectedItem != null && FileSelectorControlViewModelSource.FilesCollection.Count > 0)
        {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            {
                db.Payments.Add(new()
                {
                    OrderForeignKey = SelectedItem!.Id,
                    Currency = Currency,
                    UnitToBYNConversion = UnitToBYNConv,
                    PaymentValue = BookkeeperPrice,
                    RecordDT = RecordDT
                });
                db.SaveChanges();

                FileSelectorControlViewModelSource.AttachFilesTo(db.Payments.Single(x => x.OrderForeignKey == SelectedItem!.Id && x.RecordDT == RecordDT));

                CheckOrderCondition(db, SelectedItem!.Clone());
            }

            UpdateOrdersList();
        }
    }

    public void OrderFullyPayed()
    {
        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
        {
            db.Orders.Single(x => x.Id == SelectedItem!.Id).PaymentStatus = Payment_Status.FullyPaid;
            db.SaveChanges();
            
            CheckOrderCondition(db, SelectedItem!.Clone());
        }

        UpdateOrdersList();
    }

    private void CheckOrderCondition(DatabaseContext db, Order edited)
    {
        db.Orders
        .Where(x => x.Id == edited.Id)
        .Include(x => x.Payments)
        .First()
        .Do(x =>
        {
            if (x.PaymentStatus != Payment_Status.FullyPaid)
            {
                if (x.Payments.Select(s => s.PaymentValue).Sum() >= x.PriceTotal)
                    x.PaymentStatus = Payment_Status.FullyPaid;
                else
                if (x.Payments.Select(s => s.PaymentValue).Sum() > 0)
                    x.PaymentStatus = Payment_Status.PartiallyPaid;
            }

            switch (x.PaymentStatus)
            {
                case Payment_Status.PartiallyPaid:
                    {
                        if (x.Payments.Select(s => s.PaymentValue).Sum() >= x.PriceRequired)
                            x.TaskStatus = x.TaskStatus == Task_Status.AwaitPayment ? Task_Status.AwaitStart : x.TaskStatus;
                    };
                    break;

                case Payment_Status.FullyPaid:
                    {
                        x.TaskStatus = x.TaskStatus == Task_Status.AwaitPayment ? Task_Status.AwaitStart : x.TaskStatus;
                    };
                    break;

                default:
                    break;
            }
        });

        db.SaveChanges();
    }
}    

public class OrdersObservableControlViewModel : ViewModelBase<Order, OrdersObservableControlViewModelSource>
{
    public IObservable<bool>? IfFullyPayed { get; protected set; }

    public IObservable<bool>? IfOrderIsSelected { get; protected set; }

    public ReactiveCommand<Unit, Unit>? UpdateOrsersListCommand { get; protected set; }

    public ReactiveCommand<string, Unit>? OpenFileCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? PaymentPartialAccept { get; protected set; }

    public ReactiveCommand<Unit, Unit>? PaymentFullAccept { get; protected set; }

    public OrdersObservableControlViewModel() : base(new() { PagesCount = 1 })
    {
        IfFullyPayed = this.WhenAnyValue(
            x => x.Source.SelectedItem,
            x => x.Source.SelectedItem!.PriceTotal,
            x => x.Source.SelectedItem!.PricePaid,
            x => x.Source.SelectedItem!.PaymentStatus,
            (item, p_total, p_paid, p_status) =>
                item is not null &&
                p_paid >= p_total &&
                p_status != Payment_Status.FullyPaid
        );

        IfOrderIsSelected = this.WhenAnyValue(
            x => x.Source.SelectedItem,    
            x => x.Source.SelectedItem!.PaymentStatus,
            (item, p_status) =>
                item is not null &&
                p_status != Payment_Status.FullyPaid
        );

        UpdateOrsersListCommand = ReactiveCommand.Create(Source.UpdateOrdersList);

        OpenFileCommand = ReactiveCommand.Create<string>(x => {
            App.Host!.Services.GetService<FilesContainerService>()!.OpenFile(x);
        });

        PaymentPartialAccept = ReactiveCommand.Create(Source.PaymentAccept, IfOrderIsSelected);
        
        PaymentFullAccept = ReactiveCommand.Create(Source.OrderFullyPayed, IfFullyPayed);

        if (!Design.IsDesignMode)
        {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit())) db
                .DoInst(x => Source.CurrencyUnits = new(db.CurrencyUnits.Select(y => y.Name)))
                .Do(x =>
                {
                    Source.DtSortBegin = x.Orders
                        .Where(s => s.PaymentStatus == Payment_Status.Unpaid || s.PaymentStatus == Payment_Status.PartiallyPaid)
                        .DoIf(x => { }, x => x.Count() > 0)?
                        .Select(s => s.RecordDT.Date)
                        .Min() ?? DateTime.Today;
                });

            Source.UpdateOrdersList();
        }
    }
}
