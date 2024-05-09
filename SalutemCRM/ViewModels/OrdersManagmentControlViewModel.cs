using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class OrdersManagmentControlViewModelSource : ReactiveControlSource<Order>
{
    [ObservableProperty]
    private ObservableCollection<Order> _ordersCollection = new();


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentSelector))]
    private bool _isOnlyFullyPaid = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentSelector))]
    private bool _isOnlyNotPaid = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentSelector))]
    private bool _isOnlyPartialPaid = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskSelector))]
    private bool _isNotAvailable = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskSelector))]
    private bool _isAwaitPayment = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskSelector))]
    private bool _isAwaitStart = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskSelector))]
    private bool _isExecution = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskSelector))]
    private bool _isFinished = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskSelector))]
    private bool _isCancelled = false;


    [ObservableProperty]
    private DateTime _dtSortBegin = DateTime.Today;

    [ObservableProperty]
    private DateTime _dtSortEnd = DateTime.Today.AddDays(1);


    public List<Payment_Status> PaymentSelector => new List<Payment_Status>()
        .Do(x => { if (IsOnlyFullyPaid)     x.Add(Payment_Status.FullyPaid); })
        .Do(x => { if (IsOnlyNotPaid)       x.Add(Payment_Status.Unpaid); })
        .Do(x => { if (IsOnlyPartialPaid)   x.Add(Payment_Status.PartiallyPaid); });

    public List<Task_Status> TaskSelector => new List<Task_Status>()
        .Do(x => { if (IsNotAvailable)     x.Add(Task_Status.NotAvailable); })
        .Do(x => { if (IsAwaitPayment)     x.Add(Task_Status.AwaitPayment); })
        .Do(x => { if (IsAwaitStart)       x.Add(Task_Status.AwaitStart); })
        .Do(x => { if (IsExecution)        x.Add(Task_Status.Execution); })
        .Do(x => { if (IsFinished)         x.Add(Task_Status.Finished); })
        .Do(x => { if (IsCancelled)        x.Add(Task_Status.Cancelled); });


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
                    .Include(x => x.Manufactures)
                        .ThenInclude(x => x.OrderProcesses)
                    .Include(x => x.WarehouseSupplies)
                    .Include(x => x.MaterialFlows)
                        .ThenInclude(x => x.WarehouseItem)
                    .Include(x => x.MaterialFlows)
                        .ThenInclude(x => x.WarehouseSupply)
                    .Include(x => x.Payments)
                        .ThenInclude(x => x.FileAttachs)
                where (PaymentSelector.Any(s => item.PaymentStatus == s) &&
                       TaskSelector.Any(s => item.TaskStatus == s)) &&
                       item.RecordDT >= DtSortBegin && item.RecordDT <= DtSortEnd
                select item
            );
        }

        if (temp is not null)
            SelectedItem = OrdersCollection.SingleOrDefault(x => x.Id == temp.Id);
    }

    public void AcceptOrderProduction()
    {
        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
        {
            db.Orders.Single(x => x.Id == SelectedItem!.Id).TaskStatus = Task_Status.AwaitStart;
            db.SaveChanges();
        }

        UpdateOrdersList();
    }
}

public class OrdersManagmentControlViewModel : ViewModelBase<Order, OrdersManagmentControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? UpdateOrsersListCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? ProductionAcceptCommand { get; protected set; }

    public OrdersManagmentControlViewModel() : base(new() { PagesCount = 1 })
    {
        UpdateOrsersListCommand = ReactiveCommand.Create(Source.UpdateOrdersList);

        ProductionAcceptCommand = ReactiveCommand.Create(Source.AcceptOrderProduction);

        if (!Design.IsDesignMode)
        {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit())) db
                .Do(x =>
                {
                    Source.DtSortBegin = x.Orders
                        .Where(s => s.TaskStatus != Task_Status.Finished && s.TaskStatus != Task_Status.Cancelled)
                        .DoIf(x => { }, x => x.Count() > 0)?
                        .Select(s => s.RecordDT.Date)
                        .Min() ?? DateTime.Today;
                });

            Source.UpdateOrdersList();
        }
    }
}
