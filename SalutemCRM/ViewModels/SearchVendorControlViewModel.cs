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

namespace SalutemCRM.ViewModels;

public partial class SearchVendorControlViewModelContext : ObservableObject
{
    public bool IsVendorSelected => SelectedVendor != null;

    [ObservableProperty]
    private bool _isResponsiveControl = false;

    [ObservableProperty]
    private bool _isFuncAddNewAvailable = true;

    [ObservableProperty]
    private bool _isFuncEditAvailable = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPage1))]
    [NotifyPropertyChangedFor(nameof(IsPage2))]
    [NotifyPropertyChangedFor(nameof(IsPage3))]
    private int _activePage = 0;

    public bool IsPage1 => ActivePage == 0;
    public bool IsPage2 => ActivePage == 1;
    public bool IsPage3 => ActivePage == 2;

    [ObservableProperty]
    private string _searchVendorInputStr = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVendorSelected))]
    private Vendor? _selectedVendor = null;

    [ObservableProperty]
    private Vendor? _editVendor = new();

    [ObservableProperty]
    private Vendor? _editTempVendor = new();

    [ObservableProperty]
    private Vendor? _newVendor = new();

    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors = new() {
        new Vendor() { Name = "Test 1", Address = "*** address ***" },
        new Vendor() { Name = "Test 2", Address = "*** address ***" },
        new Vendor() { Name = "Test 3", Address = "*** address ***" },
        new Vendor() { Name = "Test 4", Address = "*** address ***" },
        new Vendor() { Name = "Test 5", Address = "*** address ***" }
    };

    public event SearchHandler? Notify = null;

    partial void OnSearchVendorInputStrChanged(string? oldValue, string? newValue) => Notify?.Do(x => x(SearchVendorInputStr));
}

public class SearchVendorControlViewModel : ViewModelBase
{
    private SearchVendorControlViewModelContext _context = new();
    public SearchVendorControlViewModelContext Context
    {
        get => _context;
        set => this.RaiseAndSetIfChanged(ref _context, value);
    }

    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public ReactiveCommand<Unit, Unit> GoAddVendor { get; }
    public ReactiveCommand<Vendor, Unit> GoEditVendor { get; }
    public ReactiveCommand<Unit, Unit> AddNewVendor { get; }
    public ReactiveCommand<Unit, Unit> EditVendor { get; }

    public SearchVendorControlViewModel()
    {
        Context.Notify += SearchVendorByInput;

        IObservable<bool> _ifNewVendorFilled = this.WhenAnyValue(
            x => x.Context.NewVendor, x => x.Context.NewVendor.Name, x => x.Context.NewVendor.Address,
            (obj, n, a) =>
                obj != null &&
                !string.IsNullOrWhiteSpace(n) &&
                !string.IsNullOrWhiteSpace(a) &&
                n.Length >= 5 &&
                a.Length >= 5
        );

        IObservable<bool> _ifEditVendorFilled = this.WhenAnyValue(
            x => x.Context.EditVendor.Name, x => x.Context.EditVendor.Address, x => x.Context.EditTempVendor.Name, x => x.Context.EditTempVendor.Address,
            (on, oa, nn, na) =>
                !string.IsNullOrWhiteSpace(on) &&
                !string.IsNullOrWhiteSpace(oa) &&
                !string.IsNullOrWhiteSpace(nn) &&
                !string.IsNullOrWhiteSpace(na) &&
                nn.Length >= 5 &&
                na.Length >= 5 &&
                (on != nn || oa != na)
        );

        GoBack = ReactiveCommand.Create(() => {
            Context.ActivePage = 0;
        });

        GoAddVendor = ReactiveCommand.Create(() => {
            Context.ActivePage = 1;
            Context.NewVendor = new();
        });

        GoEditVendor = ReactiveCommand.Create<Vendor>(x => {
            Context.ActivePage = 2;
            Context.EditVendor = x.Clone() as Vendor;
            Context.EditTempVendor = x.Clone() as Vendor;
            Context.EditTempVendor!.AdditionalInfo = "";
        });

        AddNewVendor = ReactiveCommand.Create(() => {
            Context
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    db.Vendors.Add(x.NewVendor!);
                    db.SaveChanges();
                };
            }, x => x.NewVendor != null)?
            .DoInst(x => x.SearchVendorInputStr = x.NewVendor!.Name)
            .DoInst(x => x.NewVendor = new())
            .DoInst(x => x.ActivePage = 0);
        }, _ifNewVendorFilled);

        EditVendor = ReactiveCommand.Create(() => {
            Context
            .Do(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    Vendor? vendor = db.Vendors.Single(v => v.Name == x.EditVendor!.Name);
                    vendor.Name = x.EditTempVendor!.Name;
                    vendor.Address = x.EditTempVendor!.Address;
                    vendor.AdditionalInfo +=
                        $"\n\n{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}, \n...пользователь..." +
                        $"\n{((x.EditTempVendor!.AdditionalInfo ?? "").Length > 0 ? x.EditTempVendor!.AdditionalInfo : "*тихо внесена правка*")}";

                    db.SaveChanges();
                };
            })?
            .DoInst(x => x.SearchVendorInputStr = x.EditTempVendor!.Name)
            .DoInst(x => x.ActivePage = 0);
        }, _ifEditVendorFilled);


        if (!Design.IsDesignMode)
            SearchVendorByInput("");
    }

    public SearchVendorControlViewModel(SearchVendorControlViewModelContext UIConfig) : this() => this.Context = UIConfig;

    public void SearchVendorByInput(string keyword)
    {
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Context.Vendors = db.Vendors
                .Do(x => x.Where(v => v.Name.ToLower().Contains(keyword.ToLower()) || v.Address.ToLower().Contains(keyword.ToLower())))
                .Do(x => new ObservableCollection<Vendor>(x.ToList()));
    }
}
