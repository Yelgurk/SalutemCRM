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
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class CRUSCountryControlViewModelSource : ReactiveControlSource<Country>
{
    [ObservableProperty]
    private ObservableCollection<Country> _countries = new() {
        new Country() { Name = "Test 1" },
        new Country() { Name = "Test 2" },
        new Country() { Name = "Test 3" },
        new Country() { Name = "Test 4" },
        new Country() { Name = "Test 5" }
    };

    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Countries = new(
                from c in db.Countries.Include(x => x.Cities).AsEnumerable()
                where keyword.Split(" ").Any(s => c.Name.ToLower().Contains(s))
                select c
            );
    }
}

public class CRUSCountryControlViewModel : ViewModelBase<Country, CRUSCountryControlViewModelSource>
{
    public CRUSCountryControlViewModel() : base(new() { PagesCount = 3 })
    {
        IfNewFilled = this.WhenAnyValue(
            x => x.Source.TempItem,
            x => x.Source.TempItem!.Name,
            (obj, name) =>
                obj != null &&
                !string.IsNullOrWhiteSpace(name) &&
                name.Length >= 2
        );

        IfEditFilled = this.WhenAnyValue(
            x => x.Source.EditItem!.Name,
            x => x.Source.TempItem!.Name,
            (old_name, new_name) =>
                old_name != new_name &&
                !string.IsNullOrWhiteSpace(new_name) &&
                new_name.Length >= 2
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

        GoEditCommand = ReactiveCommand.Create<Country>(x => {
            Source
                .DoInst(s => s.EditItem = x.Clone())
                .DoInst(s => s.TempItem = x.Clone())
                .Do(s => s.SetActivePage(2));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    db.Countries.Add(x.TempItem!);
                    db.SaveChanges();
                };
            }, x => x.TempItem != null)?
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .DoInst(x => x.TempItem = new())
            .Do(x => x.SetActivePage(0));
        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {
            Source
            .Do(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    Country? country = db.Countries
                        .Where(c => c.Id == x.EditItem!.Id)
                        .First();

                    country.Name = x.TempItem!.Name;
                    db.SaveChanges();
                };
            })
            .DoInst(x => x.DoIf(s => Source.SearchByInput(s.TempItem!.Name), s => s.SearchInputStr == s.TempItem!.Name))
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .Do(x => x.SetActivePage(0));
        }, IfEditFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source.SearchByInput("");

        Source.SetActivePage(0);
    }
}
