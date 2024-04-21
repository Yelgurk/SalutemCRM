using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using SalutemCRM.Domain.Model;
using SalutemCRM.ViewModels;
using System.Diagnostics;

namespace SalutemCRM.ControlTemplated;

public class OrderEditorControl : TemplatedControl
{
    public static readonly DirectProperty<OrderEditorControl, Order?> OrderSourceProperty =
    AvaloniaProperty.RegisterDirect<OrderEditorControl, Order?>(
        nameof(OrderSource),
        o => o.OrderSource,
        (o, v) => o.OrderSource = v);

    private Order? _orderSource = null;

    public Order? OrderSource
    {
        get { return _orderSource; }
        set { SetAndRaise(OrderSourceProperty, ref _orderSource, value); }
    }



    public static readonly DirectProperty<OrderEditorControl, OrderEditorControlViewModel> VMProperty =
    AvaloniaProperty.RegisterDirect<OrderEditorControl, OrderEditorControlViewModel>(
       nameof(VM),
       o => o.VM);

    private OrderEditorControlViewModel _vm = new();

    public OrderEditorControlViewModel VM => _vm;
}
