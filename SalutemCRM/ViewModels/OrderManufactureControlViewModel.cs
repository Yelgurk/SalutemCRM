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

namespace SalutemCRM.ViewModels;

public partial class OrderManufactureControlViewModelSource : ReactiveControlSource<Order>
{
}

public class OrderManufactureControlViewModel : ViewModelBase<Order, OrderManufactureControlViewModelSource>
{
    public OrderManufactureControlViewModel() : base(new() { PagesCount = 1 })
    {

    }
}
