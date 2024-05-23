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

    public OrderManufactureControlViewModelSource() => SelectedItemChangedTrigger += _ =>
    {
        HideAllOverlays();
        SelectedProduct = null;
        OrderOnPrep = null;

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
                .Include(x => x.Employee)
                    .ThenInclude(x => x.OrderProcesses)
                .First();
    };

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
        if (IsMaterialTakenFromTemplate && CRUSProductTemplateControlViewModelSource.GlobalContainer.SelectedItem is not null)
        {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                db.ProductTemplates
                    .Where(x => x.Id == CRUSProductTemplateControlViewModelSource.GlobalContainer.SelectedItem.Id)
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
                                WarehouseItem = x,
                                WarehouseItemForeignKey = x!.Id
                            });
                    });

            HideAllOverlays();
        }
        else if (!IsMaterialTakenFromTemplate)
        {
        }
    }

    public void AddEmployeeTaskIntoProduction()
    {
    }
}

public class OrderManufactureControlViewModel : ViewModelBase<Order, OrderManufactureControlViewModelSource>
{
    public IObservable<bool>? IsProductSelected { get; protected set; }

    public IObservable<bool>? IsEmployeeTaskSelected { get; protected set; }

    public ReactiveCommand<Unit, Unit>? ShowOverlayAddTaskCommand { get; protected set; }
    
    public ReactiveCommand<Unit, Unit>? ShowOverlayAddMaterialCommand { get; protected set; }
    
    public ReactiveCommand<Unit, Unit>? HideOverlaysCommand { get; protected set; }

    public ReactiveCommand<MaterialFlow, Unit>? RemoveMaterialFromBuilderCommand { get; protected set; }

    public ReactiveCommand<MaterialFlow, Unit>? RunMaterialRotationCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddMaterialsIntoProductionCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddEmployeeTaskIntoProductionCommand { get; protected set; }

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
    }
}
