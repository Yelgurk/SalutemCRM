using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SalutemCRM.Control;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class WarehouseKeeperOrder : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsReceiveNewMaterials))]
    [NotifyPropertyChangedFor(nameof(IsProvideMaterialsForService))]
    [NotifyPropertyChangedFor(nameof(IsProvideMaterialsForManufacture))]
    [NotifyPropertyChangedFor(nameof(IsProvideMaterials))]
    [NotifyPropertyChangedFor(nameof(IsAvailabilityCheck))]
    [NotifyPropertyChangedFor(nameof(CardOrderHeader))]
    [NotifyPropertyChangedFor(nameof(CardAimHeader))]
    private Order_Type _orderType;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAvailabilityCheck))]
    private Order? _order;

    [ObservableProperty]
    private Manufacture? _manufacture;

    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _materialsIn = new();

    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialsOut = new();

    public bool IsReceiveNewMaterials => OrderType == Order_Type.WarehouseRestocking;
    public bool IsProvideMaterialsForService => OrderType == Order_Type.CustomerService;
    public bool IsProvideMaterialsForManufacture => OrderType == Order_Type.ManagerSale;
    public bool IsAvailabilityCheck => OrderType == Order_Type.CustomerService && (Order?.TaskStatus ?? Task_Status.AwaitStart) == Task_Status.NotAvailable;
    public bool IsProvideMaterials => IsProvideMaterialsForService || IsProvideMaterialsForManufacture;

    public string CardOrderHeader => OrderType switch
    {
        Order_Type.WarehouseRestocking => $"Счёт #{Order!.Id}",
        Order_Type.CustomerService => $"Счёт #{Order!.Id}",
        Order_Type.ManagerSale => $"Изделие {(Manufacture!.HaveSerialNumber ? $"#{Manufacture.Code}" : $"{Manufacture.Name}")}",
        _ => "Ошибка, к админу!"
    };

    public string CardAimHeader => OrderType switch
    {
        Order_Type.WarehouseRestocking => $"Приём на склад",
        Order_Type.CustomerService => IsAvailabilityCheck ? "Проверка наличия" : $"Выдача в сервис",
        Order_Type.ManagerSale => $"Выдача в производство",
        _ => "Ошибка, к админу!"
    };
}

public partial class WarehouseKeeperOrdersViewModelSource : ReactiveControlSource<WarehouseKeeperOrder>
{
    [ObservableProperty]
    private ObservableCollection<WarehouseKeeperOrder> _warehouseKeeperOrders = new();

    public void Update()
    {
        WarehouseKeeperOrders.Clear();

        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
        {
            /* warehouse buyer material flow */
            db.Orders
                .Include(x => x.WarehouseSupplies)
                .Where(x =>
                    x.OrderType == Order_Type.WarehouseRestocking &&
                    (x.TaskStatus == Task_Status.AwaitStart || x.TaskStatus == Task_Status.Execution))
                .DoForEach(x => WarehouseKeeperOrders.Add(new()
                {
                    OrderType = Order_Type.WarehouseRestocking,
                    Order = x,
                    MaterialsIn = new(x.WarehouseSupplies.Where(s => s.DeliveryStatus == Delivery_Status.NotDelivered))
                }));

            /* service center material flow */
            db.Orders
                .Include(x => x.MaterialFlows)
                    .ThenInclude(x => x.WarehouseItem)
                        .ThenInclude(x => x!.Category)
                .Where(x =>
                    x.OrderType == Order_Type.CustomerService &&
                    (x.TaskStatus == Task_Status.Execution || x.TaskStatus == Task_Status.NotAvailable) &&
                    x.MaterialFlows.Any(s => s.DeliveryStatus < Delivery_Status.FullyDelivered))
                .DoForEach(x => WarehouseKeeperOrders.Add(new()
                {
                    OrderType = Order_Type.CustomerService,
                    Order = x,
                    MaterialsOut = new(x.MaterialFlows.Where(s => s.DeliveryStatus < Delivery_Status.FullyDelivered))
                }));

            /* material flow of manufacture for manager sales */
            db.OrderProcesses
                .Include(x => x.Order)
                .Include(x => x!.Manufacture)
                    .ThenInclude(x => x!.OrderProcesses)
                .Include(x => x!.Manufacture)
                    .ThenInclude(x => x!.MaterialFlows)
                        .ThenInclude(x => x!.WarehouseItem)
                            .ThenInclude(x => x!.Category)
                .Where(x =>
                    (x.StartedDT == null || x.CompletedDT == null) &&
                    x.EmployeeForeignKey == Account.Current.User.Id &&
                    x.Manufacture!.Order!.TaskStatus == Task_Status.Execution)
                .ToList()
                .Where(x =>
                {
                    OrderProcess? _match = x.Manufacture!.OrderProcesses.GetPrevious(x.Manufacture!.OrderProcesses.Single(s => s.Id == x.Id));
                    return _match is null || _match.CompletedDT is not null;
                })
                .DoForEach(x => WarehouseKeeperOrders.Add(new()
                {
                    OrderType = Order_Type.ManagerSale,
                    Manufacture = x.Manufacture,
                    MaterialsOut = new(x.Manufacture!.MaterialFlows)
                }));
        }
    }

    public void GoToProvidingMaterials(WarehouseKeeperOrder _selected)
    {
        NavigationViewModelSource.SetNonRegWindowContent<WarehouseProvideMaterialsControl>();
        App.Host!.Services.GetService<WarehouseProvideMaterialsControlViewModel>()!.Source.SelectedItem = _selected;
    }

    public void GoToReceivingMaterials(WarehouseKeeperOrder _selected)
    {
        NavigationViewModelSource.SetNonRegWindowContent<WarehouseReceiveMaterialsControl>();
        App.Host!.Services.GetService<WarehouseReceiveMaterialsControlViewModel>()!.Source.SelectedItem = _selected;
    }

    public void AcceptMaterialsAvailability(WarehouseKeeperOrder _accept)
    {
        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            db.Orders
                .DoIf(x => { }, x => _accept.OrderType == Order_Type.CustomerService)?
                .SingleOrDefault(s => s.Id == _accept.Order!.Id)?
                .Do(x => x.TaskStatus = Task_Status.AwaitPayment)
                .Do(x => db.SaveChanges());

        this.Update();
    }
}

public class WarehouseKeeperOrdersViewModel : ViewModelBase<WarehouseKeeperOrder, WarehouseKeeperOrdersViewModelSource>
{
    public ReactiveCommand<WarehouseKeeperOrder, Unit>? GoToProvidingMaterialsCommand { get; protected set; }

    public ReactiveCommand<WarehouseKeeperOrder, Unit>? GoToReceivingMaterialsCommand { get; protected set; }

    public ReactiveCommand<WarehouseKeeperOrder, Unit>? AcceptMaterialsAvailabilityCommand { get; protected set; }

    public WarehouseKeeperOrdersViewModel() : base(new() { PagesCount = 1 })
    {
        if (!Design.IsDesignMode)
            Source.Update();

        GoToProvidingMaterialsCommand = ReactiveCommand.Create<WarehouseKeeperOrder>(Source.GoToProvidingMaterials);

        GoToReceivingMaterialsCommand = ReactiveCommand.Create<WarehouseKeeperOrder>(Source.GoToReceivingMaterials);

        AcceptMaterialsAvailabilityCommand = ReactiveCommand.Create<WarehouseKeeperOrder>(Source.AcceptMaterialsAvailability);
    }
}
