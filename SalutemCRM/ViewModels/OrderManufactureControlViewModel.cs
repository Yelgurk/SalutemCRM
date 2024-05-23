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

namespace SalutemCRM.ViewModels;

public partial class OrderManufactureControlViewModelSource : ReactiveControlSource<Order>
{
    [ObservableProperty]
    private Order? _orderOnPrep;

    [ObservableProperty]
    private Manufacture? _selectedProduct;

    [ObservableProperty]
    private bool _isOverlayAddTask = false;

    [ObservableProperty]
    private bool _isOverlayAddMaterial = false;

    public OrderManufactureControlViewModelSource() => SelectedItemChangedTrigger += _ =>
    {
        IsOverlayAddMaterial = IsOverlayAddTask = false;

        if (SelectedItem == null)
            OrderOnPrep = null;
        else
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
}

public class OrderManufactureControlViewModel : ViewModelBase<Order, OrderManufactureControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? ShowOverlayAddTaskCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? ShowOverlayAddMaterialCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? HideOverlaysCommand { get; protected set; }

    public OrderManufactureControlViewModel() : base(new() { PagesCount = 1 })
    {
        GoBackCommand = ReactiveCommand.Create(NavigationViewModelSource.SetRegisteredWindowContent<OrdersManagmentControl>);

        ShowOverlayAddTaskCommand = ReactiveCommand.Create(() => { Source.IsOverlayAddTask = true; });

        ShowOverlayAddMaterialCommand = ReactiveCommand.Create(() => { Source.IsOverlayAddMaterial = true; });

        HideOverlaysCommand = ReactiveCommand.Create(() =>
        {
            Source.IsOverlayAddTask = false;
            Source.IsOverlayAddMaterial = false;
        });
    }
}
