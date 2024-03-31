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

namespace SalutemCRM.ViewModels;

public class SearchVendorControlViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    public Vendor? SelectedVendor { get; set; } = null;

    private ObservableCollection<Vendor> _vendors = new() {
        new Vendor() { Name = "Test 1", Address = "*** address ***" },
        new Vendor() { Name = "Test 2", Address = "*** address ***" },
        new Vendor() { Name = "Test 3", Address = "*** address ***" },
        new Vendor() { Name = "Test 4", Address = "*** address ***" },
        new Vendor() { Name = "Test 5", Address = "*** address ***" }
    };
    public ObservableCollection<Vendor> Vendors
    {
        get => _vendors;
        set => this.RaiseAndSetIfChanged(ref _vendors, value);
    }

    private string _searchVendorInputStr = "";
    public string SearchVendorInputStr
    {
        get => _searchVendorInputStr;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchVendorInputStr, value);

            if (value.Length == 0)
                SearchVendorAll();
            else
                SearchVendorByInput();
        }
    }

    public SearchVendorControlViewModel()
    {
        SearchCommand = ReactiveCommand.Create(SearchVendorByInput);

        if (!Design.IsDesignMode)
            SearchVendorAll();
    }

    private void SearchVendorAll()
    {
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Vendors = db.Vendors
                .Do(x => x.Load())
                .Do(x => new ObservableCollection<Vendor>(x.ToList()));
    }

    private void SearchVendorByInput()
    {
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Vendors = db.Vendors
                .Do(x => x.Where(v => v.Name.ToLower().Contains(SearchVendorInputStr.ToLower()) || v.Address.ToLower().Contains(SearchVendorInputStr.ToLower())))
                .Do(x => new ObservableCollection<Vendor>(x.ToList()));
    }
}
