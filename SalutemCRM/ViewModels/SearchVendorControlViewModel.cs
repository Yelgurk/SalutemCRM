﻿using Avalonia.Controls.Models.TreeDataGrid;
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

namespace SalutemCRM.ViewModels;

public partial class SearchVendorControlViewModelSource : ReactiveControlSource<Vendor>
{
    public SearchVendorControlViewModelSource() : base(3) { }

    [ObservableProperty]
    private bool _isFuncAddNewAvailable = true;

    [ObservableProperty]
    private bool _isFuncEditAvailable = true;

    [ObservableProperty]
    private string _searchVendorInputStr = "";

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
    public SearchVendorControlViewModelSource Source { get; } = new();

    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
    public ReactiveCommand<Unit, Unit> GoAddVendorCommand { get; }
    public ReactiveCommand<Vendor, Unit> GoEditVendorCommand { get; }
    public ReactiveCommand<Unit, Unit> AddNewVendorCommand { get; }
    public ReactiveCommand<Unit, Unit> EditVendorCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearSearchCommand { get; }

    public SearchVendorControlViewModel()
    {
        Source.Notify += SearchVendorByInput;



        IObservable<bool> _ifNewVendorFilled = this.WhenAnyValue(
            x => x.Source.NewVendor, x => x.Source.NewVendor!.Name, x => x.Source.NewVendor!.Address,
            (obj, n, a) =>
                obj != null &&
                !string.IsNullOrWhiteSpace(n) &&
                !string.IsNullOrWhiteSpace(a) &&
                n.Length >= 5 &&
                a.Length >= 5
        );

        IObservable<bool> _ifEditVendorFilled = this.WhenAnyValue(
            x => x.Source.EditVendor!.Name, x => x.Source.EditVendor!.Address, x => x.Source.EditTempVendor!.Name, x => x.Source.EditTempVendor!.Address,
            (on, oa, nn, na) =>
                !string.IsNullOrWhiteSpace(on) &&
                !string.IsNullOrWhiteSpace(oa) &&
                !string.IsNullOrWhiteSpace(nn) &&
                !string.IsNullOrWhiteSpace(na) &&
                nn.Length >= 5 &&
                na.Length >= 5 &&
                (on != nn || oa != na)
        );

        IObservable<bool> _ifSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchVendorInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );



        GoBackCommand = ReactiveCommand.Create(() => {
            Source.SetActivePage(0);
        });

        GoAddVendorCommand = ReactiveCommand.Create(() => {
            Source.SetActivePage(1);
            Source.NewVendor = new();
        });

        GoEditVendorCommand = ReactiveCommand.Create<Vendor>(x => {
            Source.SetActivePage(2);
            Source.EditVendor = x.Clone() as Vendor;
            Source.EditTempVendor = x.Clone() as Vendor;
            Source.EditTempVendor!.AdditionalInfo = "";
        });

        AddNewVendorCommand = ReactiveCommand.Create(() => {
            Source
            .DoIf(x => {
                using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
                {
                    db.Vendors.Add(x.NewVendor!);
                    db.SaveChanges();
                };
            }, x => x.NewVendor != null)?
            .DoInst(x => x.SearchVendorInputStr = x.NewVendor!.Name)
            .DoInst(x => x.NewVendor = new())
            .Do(x => x.SetActivePage(0));
        }, _ifNewVendorFilled);

        EditVendorCommand = ReactiveCommand.Create(() => {
            Source
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
            .Do(x => x.SetActivePage(0));
        }, _ifEditVendorFilled);

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchVendorInputStr = "";
        }, _ifSearchStrNotNull);



        if (!Design.IsDesignMode)
            SearchVendorByInput("");

        Source.SetActivePage(0);
    }

    public void SearchVendorByInput(string keyword)
    {
        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            Source.Vendors = db.Vendors
                .Do(x => x.Where(vendor => vendor.Name.ToLower().Contains(keyword.ToLower()) || vendor.Address.ToLower().Contains(keyword.ToLower())))
                .Do(x => x.Include(vendor => vendor.Orders))
                .Do(x => new ObservableCollection<Vendor>(x.ToList()));
    }
}
