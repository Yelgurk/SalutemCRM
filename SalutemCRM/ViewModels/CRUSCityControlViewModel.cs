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

public partial class CRUSCityControlViewModelSource : ReactiveControlSource<City>
{
    [ObservableProperty]
    private ObservableCollection<City> _cities = new() {
        new City() { Name = "Test 1" },
        new City() { Name = "Test 2" },
        new City() { Name = "Test 3" },
        new City() { Name = "Test 4" },
        new City() { Name = "Test 5" }
    };

    [ObservableProperty]
    private ObservableCollection<Country> _countries = new();

    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Cities = new(
                from c in db.Cities
                    .Include(x => x.Country)
                    .Include(x => x.Vendors)
                    .Include(x => x.Clients)
                    .AsEnumerable()
                where keyword.Split(" ").Any(s => c.Name.ToLower().Contains(s))
                select c
            );
    }
}

public class CRUSCityControlViewModel : ViewModelBase<City>
{
    public CRUSCityControlViewModelSource Source { get; } = new() { PagesCount = 3 };

    public CRUSCityControlViewModel()
    {
        IfNewFilled = this.WhenAnyValue(
            x => x.Source.TempItem,
            x => x.Source.TempItem!.Name,
            x => x.Source.TempItem!.Country,
            (obj, name, country) =>
                country != null &&
                obj != null &&
                !string.IsNullOrWhiteSpace(name) &&
                name.Length >= 2
        );

        IfEditFilled = this.WhenAnyValue(
            x => x.Source.EditItem!.Name,
            x => x.Source.TempItem!.Name,
            x => x.Source.EditItem!.Country,
            x => x.Source.TempItem!.Country,
            (old_name, new_name, old_c, new_c) =>
                old_c != null &&
                new_c != null &&
                !string.IsNullOrWhiteSpace(new_name) &&
                new_name.Length >= 2 &&
                (new_c.Name != old_c.Name || old_name != new_name)
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
                .Do(s => {
                    using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                        s.Countries = new(db.Countries.Where(x => x.Id > 0));
                })
                .DoInst(x => x.TempItem = new())
                .Do(x => x.SetActivePage(1));
        });

        GoEditCommand = ReactiveCommand.Create<City>(x => {
            Source
                .Do(s => {
                    using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                        s.Countries = new(db.Countries.Where(x => x.Id > 0));
                })
                .DoInst(s => s.EditItem = x.Clone())
                .DoInst(s => s.EditItem!.Country = s.Countries.Single(f => f.Name == s.EditItem!.Country!.Name))
                .DoInst(s => s.TempItem = s.EditItem!.Clone())
                .Do(s => s.SetActivePage(2));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                    new City()
                    .Do(n => n = x.TempItem!.Clone())
                    .DoInst(n => n.Country = db.Countries.Single(f => f.Id == n.Country!.Id))
                    .Do(n => db.Cities.Add(n))
                    .Do(n => db.SaveChanges());
            }, x =>
                x.TempItem != null &&
                x.TempItem!.Country != null)?
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .DoInst(x => x.TempItem = new())
            .Do(x => x.SetActivePage(0));
        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {
            Source
            .Do(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    City? city = db.Cities
                        .Where(c => c.Id == x.EditItem!.Id)
                        .First();

                    city.Name = x.TempItem!.Name;
                    city.Country = db.Countries.Single(s => s.Id == x.TempItem.Country!.Id);
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
