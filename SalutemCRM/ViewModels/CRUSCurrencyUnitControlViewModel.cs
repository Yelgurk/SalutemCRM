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
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class CRUSCurrencyUnitControlViewModelSource : ReactiveControlSource<CurrencyUnit>
{
    [ObservableProperty]
    private ObservableCollection<CurrencyUnit> _currencyUnits = new() {
        new CurrencyUnit() { Name = "Test 1" },
        new CurrencyUnit() { Name = "Test 2" },
        new CurrencyUnit() { Name = "Test 3" },
        new CurrencyUnit() { Name = "Test 4" },
        new CurrencyUnit() { Name = "Test 5" }
    };

    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            CurrencyUnits = new(
                from c in db.CurrencyUnits.AsEnumerable()
                where keyword.Split(" ").Any(s => c.Name.ToLower().Contains(s))
                select c
            );
    }
}

public class CRUSCurrencyUnitControlViewModel : ViewModelBase<CurrencyUnit>
{
    public CRUSCurrencyUnitControlViewModelSource Source { get; } = new() { PagesCount = 3 };

    public CRUSCurrencyUnitControlViewModel()
    {
        IfNewFilled = this.WhenAnyValue(
            x => x.Source.TempItem,
            x => x.Source.TempItem!.Name,
            (obj, name) =>
                obj != null &&
                !string.IsNullOrWhiteSpace(name) &&
                name.Length >= 1
        );

        IfEditFilled = this.WhenAnyValue(
            x => x.Source.EditItem!.Name,
            x => x.Source.TempItem!.Name,
            (old_name, new_name) =>
                old_name != new_name &&
                !string.IsNullOrWhiteSpace(new_name) &&
                new_name.Length >= 1
        );

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
            Source
                .DoInst(x => x.TempItem = new())
                .Do(x => x.SetActivePage(1));
        });

        GoEditCommand = ReactiveCommand.Create<CurrencyUnit>(x => {
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                Source
                .DoInst(s => db.CurrencyUnits.Remove(db.CurrencyUnits.Single(c => c.Name == x.Name)))
                .DoInst(s => db.SaveChanges())
                .Do(s => s.SearchByInput(""));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    db.CurrencyUnits.Add(x.TempItem!);
                    db.SaveChanges();
                };
            }, x => x.TempItem != null)?
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .DoInst(x => x.TempItem = new())
            .Do(x => x.SetActivePage(0));
        }, IfNewFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source.SearchByInput("");

        Source.SetActivePage(0);
    }
}
