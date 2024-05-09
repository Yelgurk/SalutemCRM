using Microsoft.EntityFrameworkCore;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagment = SalutemCRM.ViewModels.OrdersManagmentControlViewModelSource;

namespace SalutemCRM.ViewModels;

public partial class OrderManufactureControlViewModelSource : ReactiveControlSource<Order>
{
    
}

public class OrderManufactureControlViewModel : ViewModelBase<Order, OrderManufactureControlViewModelSource>
{
    public OrderManufactureControlViewModel() : base(new() { PagesCount = 1 })
    {
        OrderManagment.GlobalContainer.OnSelectedItemChangedEvent += selected => {
            if (selected is null)
                Source.SelectedItem = null;
            else
                using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                    Source.SelectedItem = db.Orders
                    .Where(x => x.Id == selected.Id)
                    .Include(x => x.Manufactures)
                        .ThenInclude(x => x.MaterialFlows)
                        .ThenInclude(x => x.WarehouseItem)
                    .Include(x => x.OrderProcesses)
                        .ThenInclude(x => x.Employee)
                    .First();
        };
    }
}
