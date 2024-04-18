using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using System;
using System.Reactive;

namespace SalutemCRM.ViewModels;

public class ViewModelBase : ReactiveObject
{
}

public abstract class ViewModelBase<T1, T2> : ReactiveObject
{
    public ViewModelBase(T2 Source)
    {
        this.Source = Source;
        App.Host!.Services
            .GetService<ViewModelSourceNotifyService>()!
            .AddVMSource((this.Source as IReactiveControlSource)!);
    }

    public required T2 Source { get; init; }

    public IObservable<bool>? IfNewFilled { get; protected set; }
    public IObservable<bool>? IfEditFilled { get; protected set; }
    public IObservable<bool>? IfSearchStrNotNull { get; protected set; }

    public ReactiveCommand<Unit, Unit>? GoBackCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? GoAddCommand { get; protected set; }
    public ReactiveCommand<T1, Unit>? GoEditCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? AddNewCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? EditCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? ClearSearchCommand { get; protected set; }
}