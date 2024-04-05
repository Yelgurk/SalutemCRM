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
using SalutemCRM.Interface;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalutemCRM.ViewModels;

public partial class SearchVendorControlViewModelSource : ReactiveControlSource<Vendor>
{
    [ObservableProperty]
    private bool _isFuncAddNewAvailable = true;

    [ObservableProperty]
    private bool _isFuncEditAvailable = true;



    [ObservableProperty]
    private string _searchVendorInputStr = "";
    
    [ObservableProperty]
    private string _newVendorContactInputStr = "";

    [ObservableProperty]
    private Vendor? _editVendor = new();

    [ObservableProperty]
    private Vendor? _editTempVendor = new();

    [ObservableProperty]
    private Vendor? _newVendor = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cities))]
    private Country? _selectedCountry;

    [ObservableProperty]
    private City? _selectedCity;


    [ObservableProperty]
    private ObservableCollection<Country> _countries = new();

    public ObservableCollection<City> Cities => SelectedCountry?.Cities ?? new ObservableCollection<City>();

    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors = new() {
        new Vendor() { Name = "Test 1", Address = "*** address ***" },
        new Vendor() { Name = "Test 2", Address = "*** address ***" },
        new Vendor() { Name = "Test 3", Address = "*** address ***" },
        new Vendor() { Name = "Test 4", Address = "*** address ***" },
        new Vendor() { Name = "Test 5", Address = "*** address ***" }
    };

    [ObservableProperty]
    private ObservableCollection<string> _contactsSplitted = new();


    public event SearchHandler? Notify = null;

    partial void OnSearchVendorInputStrChanged(string? oldValue, string? newValue) => Notify?.Do(x => x(SearchVendorInputStr));
}

public class SearchVendorControlViewModel : ViewModelBase
{
    public SearchVendorControlViewModelSource Source { get; } = new() { PagesCount = 3 };

    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
    public ReactiveCommand<Unit, Unit> GoAddVendorCommand { get; }
    public ReactiveCommand<Vendor, Unit> GoEditVendorCommand { get; }
    public ReactiveCommand<Unit, Unit> AddNewVendorCommand { get; }
    public ReactiveCommand<Unit, Unit> EditVendorCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearSearchCommand { get; }
    public ReactiveCommand<Unit, Unit> NewVednorAddContactCommand { get; }
    public ReactiveCommand<string, Unit> NewVednorDeleteContactCommand { get; }

    public SearchVendorControlViewModel()
    {
        Source.Notify += SearchVendorByInput;



        IObservable<bool> _ifNewVendorFilled = this.WhenAnyValue(
            x => x.Source.NewVendor,
            x => x.Source.NewVendor!.Name,
            x => x.Source.NewVendor!.Address,
            x => x.Source.SelectedCity,
            (obj, n, a, sc) =>
                obj != null &&
                sc != null &&
                !string.IsNullOrWhiteSpace(n) &&
                !string.IsNullOrWhiteSpace(a) &&
                n.Length >= 5 &&
                a.Length >= 5
        );

        IObservable<bool> _ifEditVendorFilled = this.WhenAnyValue(
            x => x.Source.EditVendor!.Name,
            x => x.Source.EditVendor!.Address,
            x => x.Source.EditVendor!.Contacts,
            x => x.Source.EditTempVendor!.Name,
            x => x.Source.EditTempVendor!.Address,
            x => x.Source.ContactsSplitted.Count,
            x => x.Source.ContactsSplitted,
            x => x.Source.EditVendor!.City,
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

        IObservable<bool> _ifSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchVendorInputStr,
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

        GoAddVendorCommand = ReactiveCommand.Create(() => {
            Source
                .Do(x => {
                    using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                        x.Countries = new ObservableCollection<Country>(db.Countries
                            .Where(x => x.Id > 0)
                            .Include(x => x.Cities)
                            );
                })
                .DoInst(x => x.NewVendor = new())
                .Do(x => x.ContactsSplitted.Clear())
                .Do(x => x.SetActivePage(1));
        });

        GoEditVendorCommand = ReactiveCommand.Create<Vendor>(x => {
            Source
                .DoInst(s => s.EditVendor = x.Clone() as Vendor)
                .DoInst(s => s.EditTempVendor = x.Clone() as Vendor)
                .DoInst(s => s.EditTempVendor!.AdditionalInfo = "")
                .DoInst(s => s.ContactsSplitted = new ObservableCollection<string>((s.EditTempVendor!.Contacts ?? "").Split("|")))
                .DoInst(s => s.ContactsSplitted.DoIf(c => c.Clear(), c => c.Count == 1 && c.First() == ""))
                .Do(s => {
                    using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                        s.Countries = new ObservableCollection<Country>(db.Countries
                            .Where(s => s.Id > 0)
                            .Include(s => s.Cities)
                            );
                })
                .DoInst(s => s.SelectedCountry = s.Countries.Single(country => country.Cities.Select(c => c.Name).Contains(s.EditVendor!.City!.Name)))
                .DoInst(s => s.SelectedCity = s.SelectedCountry!.Cities.Single(c => c.Name == s.EditVendor!.City!.Name))
                .Do(s => s.SetActivePage(2));
        });

        AddNewVendorCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    x.NewVendor!.City = db.Cities.Single(z => z.Id == x.SelectedCity!.Id);
                    x.NewVendor!.Contacts = string.Join("|", x.ContactsSplitted); 
                    db.Vendors.Add(x.NewVendor!);
                    db.SaveChanges();
                };
            }, x =>
                x.NewVendor != null &&
                x.SelectedCity != null
            )?
            .DoInst(x => x.SearchVendorInputStr = x.NewVendor!.Name)
            .DoInst(x => x.NewVendor = new())
            .Do(x => x.SetActivePage(0));
        }, _ifNewVendorFilled);

        EditVendorCommand = ReactiveCommand.Create(() => {
            Source
            .Do(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    Vendor? vendor = db.Vendors
                        .Where(v => v.Id == x.EditVendor!.Id)
                        .Include(v => v.City)
                        .ThenInclude(x => x!.Country)
                        .First();

                    vendor.Name = x.EditTempVendor!.Name;
                    vendor.Address = x.EditTempVendor!.Address;
                    vendor.Contacts = string.Join("|", x.ContactsSplitted);
                    vendor.City = db.Cities.Single(z => z.Id == x.SelectedCity!.Id);
                    vendor.AdditionalInfo +=
                        $"\n\n{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}, \n...пользователь..." +
                        $"\n{((x.EditTempVendor!.AdditionalInfo ?? "").Length > 0 ? x.EditTempVendor!.AdditionalInfo : "*тихо внесена правка*")}";

                    db.SaveChanges();
                };
            })
            .DoInst(x => x.DoIf(z => SearchVendorByInput(z.EditTempVendor!.Name), z => z.SearchVendorInputStr == x.EditTempVendor!.Name))
            .DoInst(x => x.SearchVendorInputStr = x.EditTempVendor!.Name)
            .Do(x => x.SetActivePage(0));
        }, _ifEditVendorFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchVendorInputStr = "";
        }, _ifSearchStrNotNull);
        
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
            SearchVendorByInput("");

        Source.SetActivePage(0);
    }

    public void SearchVendorByInput(string keyword)
    {
        
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            db.Do(f =>
            {
                Source.Vendors = new(
                from v in db.Vendors.AsEnumerable()
                    where Regex.Replace(keyword.ToLower(), @"\s+", " ").Split(" ").Any(s =>
                        v.Name.ToLower().Contains(s) ||
                        (v.Address ?? "").ToLower().Contains(s) ||
                        (v.Contacts ?? "").ToLower().Contains(s)
                    )
                    select v
                );
            })
            .Do(f =>
            {
                foreach (Vendor v in Source.Vendors)
                    v.City = db.Cities
                        .Where(c => c.Id == v.CityForeignKey)
                        .Include(c => c.Country)
                        .First();

            });
        

        /*
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Source.Vendors = db.Vendors
                .Where(vendor =>
                    vendor.Name.ToLower().Contains(keyword.ToLower()) ||
                    (vendor.Address ?? "").ToLower().Contains(keyword.ToLower()) ||
                    (vendor.Contacts ?? "").ToLower().Contains(keyword.ToLower())
                 )
                .Include(vendor => vendor.Orders)
                .Include(vendor => vendor.City)
                .ThenInclude(city => city!.Country)
                .Do(x => new ObservableCollection<Vendor>(x.ToList()));
        */
    }
}
