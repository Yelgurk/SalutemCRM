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
        o => null,
        (o, v) => o.OrderSource = v);

    public Order? OrderSource
    {
        get { return VM.Source.SelectedItem; }
        set { VM.Source.SelectedItem = value; }
    }



    public static readonly DirectProperty<OrderEditorControl, OrderEditorControlViewModel> VMProperty =
    AvaloniaProperty.RegisterDirect<OrderEditorControl, OrderEditorControlViewModel>(
       nameof(VM),
       o => o.VM);

    private OrderEditorControlViewModel _vm = new();

    public OrderEditorControlViewModel VM => _vm;



    public static readonly DirectProperty<OrderEditorControl, dynamic?> ControlInjectionUIResponsiveProperty =
    AvaloniaProperty.RegisterDirect<OrderEditorControl, dynamic?>(
       nameof(ControlInjectionUIResponsive),
       o => null,
       (o, v) => o.ControlInjectionUIResponsive = v!);

    public dynamic ControlInjectionUIResponsive
    {
        set
        {
            value.Source.IsResponsiveControl = true;
        }
    }



    public static readonly DirectProperty<OrderEditorControl, dynamic?> ControlInjectionUIAddEditLockProperty =
    AvaloniaProperty.RegisterDirect<OrderEditorControl, dynamic?>(
       nameof(ControlInjectionUIAddEditLock),
       o => null,
       (o, v) => o.ControlInjectionUIAddEditLock = v!);

    public dynamic ControlInjectionUIAddEditLock
    {
        set
        {
            value.Source.IsFuncAddNewAvailable = false;
            value.Source.IsFuncEditAvailable = false;
        }
    }
}
