using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Reactive.Linq;
using Avalonia.Media;
using Avalonia.Markup.Xaml.Converters;
using SalutemCRM.Reactive;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalutemCRM.ViewModels;

public partial class CRUSVendorControlViewModelSource : ReactiveControlSource<Vendor>
{
    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors = new() {
        new Vendor() { Name = "Test 1", Address = "*** address ***" },
        new Vendor() { Name = "Test 2", Address = "*** address ***" },
        new Vendor() { Name = "Test 3", Address = "*** address ***" },
        new Vendor() { Name = "Test 4", Address = "*** address ***" },
        new Vendor() { Name = "Test 5", Address = "*** address ***" }
    };


    
    [ObservableProperty]
    private string _newVendorContactInputStr = "";

    [ObservableProperty]
    private ObservableCollection<string> _contactsSplitted = new();

    

    public static Country DefaultCountry { get; } = new() { Id = 0, Name = "{ все }" };

    public static City DefaultCity { get; } = new() { Id = 0, Name = "{ все }", CountryForeignKey = DefaultCountry.Id, Country = DefaultCountry };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cities))]
    private Country? _selectedCountry;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchCities))]
    private Country _selectedSearchCountry = DefaultCountry;

    [ObservableProperty]
    private City? _selectedCity;

    [ObservableProperty]
    private City _selectedSearchCity = DefaultCity;



    [ObservableProperty]
    private ObservableCollection<Country> _countries = new();

    public ObservableCollection<City> Cities => SelectedCountry?.Cities ?? new ObservableCollection<City>();

    public ObservableCollection<City> SearchCities => SelectedSearchCountry?.Cities ?? new ObservableCollection<City>();



    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Vendors = new(
                from v in db.Vendors.Include(z => z.City).ThenInclude(z => z!.Country).AsEnumerable()
                where keyword.Split(" ").Any(s =>
                    v.Name.ToLower().Contains(s) ||
                    (v.Address ?? "").ToLower().Contains(s) ||
                    (v.Contacts ?? "").ToLower().Contains(s)
                //v.City!.Name.ToLower().Contains(s) ||
                //v.City!.Country!.Name.ToLower().Contains(s)
                )
                select v
            );
    }
}

public class CRUSVendorControlViewModel : ViewModelBase<Vendor, CRUSVendorControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit> NewVednorAddContactCommand { get; }
    public ReactiveCommand<string, Unit> NewVednorDeleteContactCommand { get; }

    public CRUSVendorControlViewModel() : base(new() { PagesCount = 3 })
    {
        IfNewFilled = this.WhenAnyValue(
            x => x.Source.TempItem,
            x => x.Source.TempItem!.Name,
            x => x.Source.TempItem!.Address,
            x => x.Source.SelectedCity,
            (obj, n, a, sc) =>
                obj != null &&
                sc != null &&
                !string.IsNullOrWhiteSpace(n) &&
                !string.IsNullOrWhiteSpace(a) &&
                n.Length >= 5 &&
                a.Length >= 5
        );

        IfEditFilled = this.WhenAnyValue(
            x => x.Source.EditItem!.Name,
            x => x.Source.EditItem!.Address,
            x => x.Source.EditItem!.Contacts,
            x => x.Source.TempItem!.Name,
            x => x.Source.TempItem!.Address,
            x => x.Source.ContactsSplitted.Count,
            x => x.Source.ContactsSplitted,
            x => x.Source.EditItem!.City,
            x => x.Source.SelectedCity,
            (on, oa, om, nn, na, ncnt, nm, oc, sc) =>
                sc != null &&
                !string.IsNullOrWhiteSpace(on) &&
                !string.IsNullOrWhiteSpace(oa) &&
                !string.IsNullOrWhiteSpace(nn) &&
                !string.IsNullOrWhiteSpace(na) &&
                nn.Length >= 5 &&
                na.Length >= 5 &&
                (on != nn || oa != na || oc!.Name != sc.Name || om != string.Join("|", nm))
        );

        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );

        IObservable<bool> _ifNewVendorAddContactStrNotNull = this.WhenAnyValue(
            x => x.Source.NewVendorContactInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(() => {
            Source.SetActivePage(0);
        });

        GoAddCommand = ReactiveCommand.Create(() => {
            Source
                .Do(x => {
                    using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                        x.Countries = new ObservableCollection<Country>(db.Countries
                            .Where(x => x.Id > 0)
                            .Include(x => x.Cities)
                            );
                })
                .DoInst(x => x.TempItem = new())
                .Do(x => x.ContactsSplitted.Clear())
                .Do(x => x.SetActivePage(1));
        });

        GoEditCommand = ReactiveCommand.Create<Vendor>(x => {
            Source
                .DoInst(s => s.EditItem = x.Clone())
                .DoInst(s => s.TempItem = x.Clone())
                .DoInst(s => s.TempItem!.AdditionalInfo = "")
                .DoInst(s => s.ContactsSplitted = new ObservableCollection<string>((s.TempItem!.Contacts ?? "").Split("|")))
                .DoInst(s => s.ContactsSplitted.DoIf(c => c.Clear(), c => c.Count == 1 && c.First() == ""))
                .Do(s => {
                    using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                        s.Countries = new ObservableCollection<Country>(db.Countries
                            .Where(s => s.Id > 0)
                            .Include(s => s.Cities)
                            );
                })
                .DoInst(s => s.SelectedCountry = s.Countries.Single(country => country.Cities.Select(c => c.Name).Contains(s.EditItem!.City!.Name)))
                .DoInst(s => s.SelectedCity = s.SelectedCountry!.Cities.Single(c => c.Name == s.EditItem!.City!.Name))
                .Do(s => s.SetActivePage(2));
        });

        AddNewCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    x.TempItem!.City = db.Cities.Single(z => z.Id == x.SelectedCity!.Id);
                    x.TempItem!.Contacts = string.Join("|", x.ContactsSplitted); 
                    db.Vendors.Add(x.TempItem!);
                    db.SaveChanges();
                };
            }, x =>
                x.TempItem != null &&
                x.SelectedCity != null
            )?
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .DoInst(x => x.TempItem = new())
            .Do(x => x.SetActivePage(0));
        }, IfNewFilled);

        EditCommand = ReactiveCommand.Create(() => {
            Source
            .Do(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    Vendor? vendor = db.Vendors
                        .Where(v => v.Id == x.EditItem!.Id)
                        .Include(v => v.City)
                        .ThenInclude(x => x!.Country)
                        .First();

                    vendor.Name = x.TempItem!.Name;
                    vendor.Address = x.TempItem!.Address;
                    vendor.Contacts = string.Join("|", x.ContactsSplitted);
                    vendor.City = db.Cities.Single(z => z.Id == x.SelectedCity!.Id);
                    vendor.AdditionalInfo +=
                        $"\n\n{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}, \n...пользователь..." +
                        $"\n{((x.TempItem!.AdditionalInfo ?? "").Length > 0 ? x.TempItem!.AdditionalInfo : "*тихо внесена правка*")}";

                    db.SaveChanges();
                };
            })
            .DoInst(x => x.DoIf(z => Source.SearchByInput(z.TempItem!.Name), z => z.SearchInputStr == x.TempItem!.Name))
            .DoInst(x => x.SearchInputStr = x.TempItem!.Name)
            .Do(x => x.SetActivePage(0));
        }, IfEditFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);
        
        NewVednorAddContactCommand = ReactiveCommand.Create(() =>
        {
            Source.ContactsSplitted = Source.ContactsSplitted.DoIf(x =>
            {
                x.Add(Source.NewVendorContactInputStr);
                Source.NewVendorContactInputStr = "";
            },
            x => !x.Contains(Source.NewVendorContactInputStr)) ?? Source.ContactsSplitted;
        },_ifNewVendorAddContactStrNotNull);


        NewVednorDeleteContactCommand = ReactiveCommand.Create<string>(x => {
            Source.ContactsSplitted = Source.ContactsSplitted.DoInst(c => c.Remove(x))!;
        });


        if (!Design.IsDesignMode)
            Source.SearchByInput("");

        Source.SetActivePage(0);
    }
}
