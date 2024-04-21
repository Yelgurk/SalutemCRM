using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class OrderEditorControlViewModelSource : ReactiveControlSource<Order>
{
    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
        { }
    }
}

public partial class OrderEditorControlViewModel : ViewModelBase<Order, OrderEditorControlViewModelSource>
{
    public OrderEditorControlViewModel() : base(new() { PagesCount = 3 })
    {
        //IfNewFilled = this.WhenAnyValue();

        //IfEditFilled = this.WhenAnyValue();

        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(() => {
            Source.SetActivePage(0);
        });

        GoAddCommand = ReactiveCommand.Create(() => {

        });

        GoEditCommand = ReactiveCommand.Create<Order>(x => {

        });

        AddNewCommand = ReactiveCommand.Create(() => {

        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {

        }, IfEditFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source.SearchByInput("");

        Source.SetActivePage(0);
    }
}
