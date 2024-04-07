using ReactiveUI;
using SalutemCRM.Domain.Model;
using System;
using System.Reactive;

namespace SalutemCRM.ViewModels;

public class ViewModelBase : ReactiveObject
{
}

public class ViewModelBase<T> : ReactiveObject
{
    public IObservable<bool>? IfNewFilled { get; protected set; }
    public IObservable<bool>? IfEditFilled { get; protected set; }
    public IObservable<bool>? IfSearchStrNotNull { get; protected set; }

    public ReactiveCommand<Unit, Unit>? GoBackCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? GoAddCommand { get; protected set; }
    public ReactiveCommand<T, Unit>? GoEditCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? AddNewCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? EditCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? ClearSearchCommand { get; protected set; }
}