using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.FunctionalControl;

public class FunctionalControlUIProvider : IReactiveControlResponseUI
{
    public Action<bool>? IsFuncAddNewAvailableSetter;
    public Action<bool>? IsFuncEditAvailableSetter;
    public Action<bool>? IsResponsiveControlSetter;

    public FunctionalControlUIProvider Source => this;

    public bool IsFuncAddNewAvailable
    {
        get => false;
        set => IsFuncAddNewAvailableSetter?.Invoke(value);
    }

    public bool IsFuncEditAvailable
    {
        get => false;
        set => IsFuncEditAvailableSetter?.Invoke(value);
    }

    public bool IsResponsiveControl
    {
        get => false;
        set => IsResponsiveControlSetter?.Invoke(value);
    }
}
