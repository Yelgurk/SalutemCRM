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
    public bool IsResponsiveSelected => SelectedVendor != null;

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
    private int _activePage = 1;

    public bool IsPage1 => ActivePage == 0;
    public bool IsPage2 => ActivePage == 1;
    public bool IsPage3 => ActivePage == 2;

    [ObservableProperty]
    private string _searchVendorInputStr = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsResponsiveSelected))]
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

    public SearchVendorControlViewModel()
    {
        Context.Notify += SearchVendorByInput;

        IObservable<bool> isInputValid = this.WhenAnyValue(
            x => x.Context.NewVendor,
            (vendor) =>
                vendor != null &&
                (vendor.Name ?? "").Length > 5 &&
                (vendor.Address ?? "").Length > 5
        ).DistinctUntilChanged();

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
        });

        AddNewVendor = ReactiveCommand.Create(() => {
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            {
                db.Vendors.Add(Context.NewVendor!);
                db.SaveChanges();
            }

            Context.SearchVendorInputStr = Context.NewVendor!.Name;
            Context.NewVendor = new();
            GoBack.Execute();
        }, isInputValid);

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
