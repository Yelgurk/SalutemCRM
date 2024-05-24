using Microsoft.EntityFrameworkCore;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalutemCRM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Reactive;
using SalutemCRM.Control;
using System.Collections.ObjectModel;
using SalutemCRM.Domain.Modell;
using DynamicData;

namespace SalutemCRM.ViewModels;

public partial class OrderManufactureControlViewModelSource : ReactiveControlSource<Order>
{
    [ObservableProperty]
    private Order? _orderOnPrep;

    [ObservableProperty]
    private Manufacture? _selectedProduct;

    [ObservableProperty]
    private MaterialFlow? _materialInRotation;

    [ObservableProperty]
    private bool _isOverlayAddTask = false;

    [ObservableProperty]
    private bool _isOverlayAddMaterial = false;

    [ObservableProperty]
    private bool _isOverlayMaterialRotation = false;

    [ObservableProperty]
    private bool _isMaterialTakenFromTemplate = true;

    [ObservableProperty]
    private ObservableCollection<User>? _manufactureEmployees;

    [ObservableProperty]
    private User? _selectedEmployee;

    [ObservableProperty]
    private string _tryAcceptOrderBeginningInfo = "";

    public OrderManufactureControlViewModelSource() => SelectedItemChangedTrigger += _ =>
    {
        HideAllOverlays();
        SelectedProduct = null;
        OrderOnPrep = null;
        TryAcceptOrderBeginningInfo = "";

        ManufactureEmployees?.Clear();
        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            ManufactureEmployees = new(
                from v in db.Users
                    .Include(x => x.UserRole)
                where UserRole.ManufactureEmployeeRoles.Any(x => x == v.UserRole!.Name)
                select v
            );

        if (SelectedItem != null)
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                OrderOnPrep = db.Orders
                .Where(x => x.Id == SelectedItem!.Id)
                .Include(x => x.Manufactures)
                    .ThenInclude(x => x.MaterialFlows)
                        .ThenInclude(x => x.WarehouseItem)
                            .ThenInclude(x => x!.Category)
                .Include(x => x.Manufactures)
                    .ThenInclude(x => x.OrderProcesses)
                        .ThenInclude(x => x.Employee)
                .Include(x => x.Manufactures)
                    .ThenInclude(x => x.OrderProcesses)
                        .ThenInclude(x => x.OrderDuty)
                .Include(x => x.Employee)
                .First();
    };

    partial void OnSelectedProductChanged(Manufacture? value) => ReIndexProductionTasksQueue();

    public void HideAllOverlays() => false
        .Do(x => IsOverlayAddMaterial = x)
        .Do(x => IsOverlayAddTask = x)
        .Do(x => IsOverlayMaterialRotation = x);

    public void RemoveMaterialFromBuilder(MaterialFlow _remove) => SelectedProduct?.Do(x => x.MaterialFlows.Remove(_remove));

    public void RunMaterialRotation(MaterialFlow _rotation)
    {
        IsOverlayMaterialRotation = true;
        MaterialInRotation = _rotation;
    }

    public void AddMaterialsIntoProduction()
    {
        if (IsMaterialTakenFromTemplate && CRUSProductTemplateControlViewModelSource.GlobalContainer.SelectedItem is ProductTemplate _prodTemplate && _prodTemplate is not null)
        {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                db.ProductTemplates
                    .Where(x => x.Id == _prodTemplate.Id)
                    .Include(x => x.ProductSchemas)
                        .ThenInclude(x => x.WarehouseItem)
                            .ThenInclude(x => x!.Category)
                    .ToList()
                    .SelectMany(x => x.ProductSchemas)
                    .Select(x => x.WarehouseItem)
                    .DoIf(x => { }, x => x.Count() > 0)?
                    .DoForEach(x =>
                    {
                        if (SelectedProduct!.MaterialFlows!.SingleOrDefault(s => s.WarehouseItem!.Id == x!.Id) is MaterialFlow match && match is not null)
                            ++match.CountReservedFromStock;
                        else
                            SelectedProduct!.MaterialFlows!.Add(new()
                            {
                                CountReservedFromStock = 1,
                                WarehouseItem = x!.Clone(),
                                WarehouseItemForeignKey = x.Clone()!.Id
                            });
                    });

            HideAllOverlays();
        }
        else if (!IsMaterialTakenFromTemplate && CRUSWarehouseItemControlViewModelSource.GlobalContainer.SelectedItem is WarehouseItem _warehouseItem && _warehouseItem is not null)
        {
            if (SelectedProduct!.MaterialFlows!.SingleOrDefault(x => x.WarehouseItem!.Id == _warehouseItem.Id) is MaterialFlow match && match is not null)
                ++match.CountReservedFromStock;
            else
                SelectedProduct!.MaterialFlows!.Add(new()
                {
                    CountReservedFromStock = 1,
                    WarehouseItem = _warehouseItem.Clone(),
                    WarehouseItemForeignKey = _warehouseItem.Clone()!.Id
                });

            HideAllOverlays();
        }
    }

    private void ReIndexProductionTasksQueue() => SelectedProduct
        .DoIf(x => { }, x => x is not null)?
        .Do(x => 1)
        .Do(x => SelectedProduct!.OrderProcesses.DoForEach(s =>
    {
        s.Queue = x++;
        s.InQueueCount = SelectedProduct!.OrderProcesses.Count();
    }));

    public void AddEmployeeTaskIntoProduction()
    {
        if (CRUSOrderDutyControlViewModelSource.GlobalContainer.SelectedItem is OrderDuty _duty && _duty is not null)
        {
            SelectedProduct!.OrderProcesses.Add(new()
            {
                Id = -1,
                Employee = SelectedEmployee!.Clone(),
                EmployeeForeignKey = SelectedEmployee!.Id,
                OrderDuty = _duty.Clone(),
                OrderDutyForeignKey = _duty.Id,
                MustBeStartedDT = DateTime.Today,
                MustBeStartedTimeSpan = TimeSpan.FromHours(8),
                DeadlineDT = DateTime.Today,
                DeadlineTimeSpan = TimeSpan.FromHours(17)
            });

            ReIndexProductionTasksQueue();
            HideAllOverlays();
        }
    }

    public void MoveOrderTaksTo(OrderProcess _task, bool _up)
    {
        if (_up)
            SelectedProduct!.OrderProcesses = new(SelectedProduct!.OrderProcesses.Move(_task, _task.Queue - 2));
        else
            SelectedProduct!.OrderProcesses = new(SelectedProduct!.OrderProcesses.Move(_task, _task.Queue + 1));

        ReIndexProductionTasksQueue();
    }

    public void RemoveOrderTask(OrderProcess _task)
    {
        SelectedProduct!.OrderProcesses.Remove(_task);
        ReIndexProductionTasksQueue();
    }

    public void AcceptOrderStartManufacturing()
    {
        bool _success = false;
        this.DoIf(x => { }, x => x.OrderOnPrep is not null)?
            .DoIf(x => { }, x => x.OrderOnPrep!.Manufactures.Count() > 0)?
            .DoIf(x => { }, x => x.OrderOnPrep!.Manufactures.Where(s => s.HaveSerialNumber).Select(s => s.Code?.Length ?? 0).Min() > 0)?
            .DoIf(x => { }, x => x.OrderOnPrep!.Manufactures.Select(s => s.MaterialFlows.Count()).Min() > 0)?
            .DoIf(x => { }, x => x.OrderOnPrep!.Manufactures.Select(s => s.OrderProcesses.Count()).Min() > 0)?
            .Do(x =>
            {
                _success = true;

                using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                {
                    var _orderUpdate = db.Orders.Single(s => s.Id == OrderOnPrep!.Id)
                        .DoInst(s => s.TaskStatus = Task_Status.Execution)
                        .DoInst(s => s.StartedDT = DateTime.Now);


                    var _alreadyTrackingManufacture =
                        (from v in db.Manufacture.AsEnumerable()
                        where OrderOnPrep!.Manufactures.Any(s => s.Id == v.Id)
                        select v)
                        .ToList();

                    /*
                    _orderUpdate.TaskStatus = Task_Status.Execution;
                    _orderUpdate.StartedDT = DateTime.Now;
                    */

                    foreach (
                        var _trace
                        in _alreadyTrackingManufacture
                        .Join(OrderOnPrep!.Manufactures, v1 => v1.Id, v2 => v2.Id, (v1, v2) => (v1, v2))
                        ) _trace.v1.Code = _trace.v2.Code;

                    OrderOnPrep!.Manufactures.DoForEach(_ =>
                    {
                        /* This part for "Edit manufacture params", not for "Start manufacture params" step */
                        /*
                        var _alreadyTrackingMaterials = db.MaterialFlow
                            .Where(s => s.ManufactureForeignKey == _.Id)
                            .ToList();

                        var _alreadyTrackingTasks = db.OrderProcesses
                            .Where(s => s.ManufactureForeignKey == _.Id)
                            .ToList();

                        var _untrackedMaterials = _.MaterialFlows.Where(v1 => !_alreadyTrackingMaterials.Any(v2 => v2.Id == v1.Id));
                        var _untrackedTasks = _.OrderProcesses.Where(v1 => !_alreadyTrackingTasks.Any(v2 => v2.Id == v1.Id));

                        
                        _.MaterialFlows.RemoveMany(_untrackedMaterials);
                        _.OrderProcesses.RemoveMany(_untrackedTasks);

                        foreach (var _tracableMaterial in _alreadyTrackingMaterials.Join(_.MaterialFlows, v1 => v1.Id, v2 => v2.Id, (v1, v2) => (v1, v2)))
                            _tracableMaterial.v1.CountReservedFromStock = _tracableMaterial.v2.CountReservedFromStock;
                        */

                        _.MaterialFlows.DoForEach(s => db.MaterialFlow.Add(new()
                        {
                            EmployeeForeignKey = Account.Current.User.Id,
                            ManufactureForeignKey = _.Id,
                            WarehouseItemForeignKey = s.WarehouseItemForeignKey,
                            CountReservedFromStock = s.CountReservedFromStock,
                            CountProvidedFromStock = 0,
                            CountReturnedToStock = 0,
                            AdditionalInfo = $"Установлена выдача сотрудником - {Account.Current.User.FullNameWithLogin}",
                            DeliveryStatus = Delivery_Status.NotDelivered,
                            RecordDT = DateTime.Now
                        }));

                        _.OrderProcesses.DoForEach(s => db.OrderProcesses.Add(new()
                        {
                            EmployeeForeignKey = s.EmployeeForeignKey,
                            ManufactureForeignKey = _.Id,
                            OrderDutyForeignKey = s.OrderDutyForeignKey,
                            Queue = s.Queue,
                            RecordDT = DateTime.Now,
                            MustBeStartedDT = s.MustBeStartedDT.AddMinutes(s.MustBeStartedTimeSpan!.Value.TotalMinutes),
                            DeadlineDT = s.DeadlineDT.AddMinutes(s.DeadlineTimeSpan!.Value.TotalMinutes),
                        }));
                    });

                    db.SaveChanges();
                }

                NavigationViewModelSource.SetRegisteredWindowContent<OrdersManagmentControl>();
            });

        TryAcceptOrderBeginningInfo = _success switch
        {
            true => "",
            _ => " [не хватает инф.]"
        };
    }
}

public class OrderManufactureControlViewModel : ViewModelBase<Order, OrderManufactureControlViewModelSource>
{
    public IObservable<bool>? IsProductSelected { get; protected set; }

    public IObservable<bool>? IsEmployeeTaskSelected { get; protected set; }

    public IObservable<bool>? IsOrderManufactureFullfilled { get; protected set; }

    public ReactiveCommand<Unit, Unit>? ShowOverlayAddTaskCommand { get; protected set; }
    
    public ReactiveCommand<Unit, Unit>? ShowOverlayAddMaterialCommand { get; protected set; }
    
    public ReactiveCommand<Unit, Unit>? HideOverlaysCommand { get; protected set; }

    public ReactiveCommand<MaterialFlow, Unit>? RemoveMaterialFromBuilderCommand { get; protected set; }

    public ReactiveCommand<MaterialFlow, Unit>? RunMaterialRotationCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddMaterialsIntoProductionCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddEmployeeTaskIntoProductionCommand { get; protected set; }

    public ReactiveCommand<OrderProcess, Unit>? MoveOrderTaksUpCommand { get; protected set; }

    public ReactiveCommand<OrderProcess, Unit>? MoveOrderTaksDownCommand { get; protected set; }

    public ReactiveCommand<OrderProcess, Unit>? RemoveOrderTaskCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AcceptOrderStartManufacturingCommand { get; protected set; }

    public OrderManufactureControlViewModel() : base(new() { PagesCount = 1 })
    {
        IsProductSelected = this.WhenAnyValue(
            x => x.Source.SelectedProduct,
            x => x.Source.SelectedProduct,
            (item1, item2) =>
                item1 is not null
        );

        IsEmployeeTaskSelected = this.WhenAnyValue(
            x => x.Source.SelectedEmployee,
            x => x.Source.SelectedEmployee,
            (item1, item2) =>
                item1 is not null
        );

        GoBackCommand = ReactiveCommand.Create(NavigationViewModelSource.SetRegisteredWindowContent<OrdersManagmentControl>);

        ShowOverlayAddTaskCommand = ReactiveCommand.Create(() => { Source.IsOverlayAddTask = true; }, IsProductSelected);

        ShowOverlayAddMaterialCommand = ReactiveCommand.Create(() => { Source.IsOverlayAddMaterial = true; }, IsProductSelected);

        HideOverlaysCommand = ReactiveCommand.Create(Source.HideAllOverlays);

        RemoveMaterialFromBuilderCommand = ReactiveCommand.Create<MaterialFlow>(Source.RemoveMaterialFromBuilder);

        RunMaterialRotationCommand = ReactiveCommand.Create<MaterialFlow>(Source.RunMaterialRotation);

        AddMaterialsIntoProductionCommand = ReactiveCommand.Create(Source.AddMaterialsIntoProduction);

        AddEmployeeTaskIntoProductionCommand = ReactiveCommand.Create(Source.AddEmployeeTaskIntoProduction, IsEmployeeTaskSelected);

        MoveOrderTaksUpCommand = ReactiveCommand.Create<OrderProcess>(x => { Source.MoveOrderTaksTo(x, true); });

        MoveOrderTaksDownCommand = ReactiveCommand.Create<OrderProcess>(x => { Source.MoveOrderTaksTo(x, false); });

        RemoveOrderTaskCommand = ReactiveCommand.Create<OrderProcess>(Source.RemoveOrderTask);

        AcceptOrderStartManufacturingCommand = ReactiveCommand.Create(Source.AcceptOrderStartManufacturing);
    }
}
