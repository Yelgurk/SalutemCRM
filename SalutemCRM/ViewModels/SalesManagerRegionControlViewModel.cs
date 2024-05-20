using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.TCP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SalutemCRM.ViewModels;

public partial class SalesManagerRegionControlViewModelSource : ReactiveControlSource<RegionMonitoring>
{
    [ObservableProperty]
    private ObservableCollection<RegionMonitoring> _existedRegions = new();

    [ObservableProperty]
    private ObservableCollection<RegionMonitoring> _tempCollection = new();

    [ObservableProperty]
    private ObservableCollection<RegionMonitoring> _removedCollection = new();

    public void Update()
    {
        bool ShowAll = Account.Current.IsRootOrBossUser;

        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            ExistedRegions = new(
                from v in db.RegionMonitorings
                    .Include(z => z.City)
                        .ThenInclude(z => z!.Country)
                    .Include(z => z.Employee)
                    .AsEnumerable()
                where ShowAll || v.EmployeeForeignKey == Account.Current.User.Id
                orderby v.EmployeeForeignKey
                select v
            );

        RemovedCollection.Clear();
        TempCollection.Clear();
        ExistedRegions.DoForEach(x => TempCollection.Add(x.Clone()));
    }

    public void AddCityToRegions() => TempCollection
        .DoIf(x => { }, x => !Account.Current.IsRootOrBossUser)?
        .DoIf(x => { }, x => ExistedRegions.Where(s => s.City!.Name == CRUSCityControlViewModelSource.GlobalContainer.SelectedItem!.Name).Count() == 0)?
        .Add(new()
        {
            City = CRUSCityControlViewModelSource.GlobalContainer.SelectedItem!.Clone(),
            CityForeignKey = CRUSCityControlViewModelSource.GlobalContainer.SelectedItem.Id,
            Employee = Account.Current.User,
            EmployeeForeignKey = Account.Current.User.Id,
            Id = -1
        });

    public void RemoveCityFromRegion(RegionMonitoring removed) => removed?
        .DoIf(x => { }, x => !Account.Current.IsRootOrBossUser)?
        .Do(x =>
        {
            if (ExistedRegions.Where(s => s.Id == x.Id).Count() > 0)
                RemovedCollection.Add(x);
        })
        .Do(TempCollection.Remove);

    public void AcceptChanges() => this
        .DoIf(x =>
        {
            try
            {
                using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                {
                    var removed = from v in db.RegionMonitorings.AsEnumerable() where RemovedCollection.Any(s => s.Id == v.Id) select v;
                    removed.DoForEach(s => db.RegionMonitorings.Remove(s));
                    db.SaveChanges();

                    TempCollection.RemoveMany(TempCollection.Where(s => ExistedRegions.Any(e => e.CityForeignKey == s.CityForeignKey)));
                    TempCollection.DoForEach(s => db.RegionMonitorings.Add(new() { CityForeignKey = s.CityForeignKey, EmployeeForeignKey = s.EmployeeForeignKey }));
                    db.SaveChanges();
                }
            }
            catch { }
        }, x => !Account.Current.IsRootOrBossUser)?
        .Do(x => this.Update());
}

public class SalesManagerRegionControlViewModel : ViewModelBase<RegionMonitoring, SalesManagerRegionControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit> AddCityToMyRegionsCommand { get; protected set; }

    public ReactiveCommand<RegionMonitoring, Unit> RemoveCityFromMyRegionsCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit> UpdateListCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit> AcceptChanges { get; protected set; }

    public SalesManagerRegionControlViewModel() : base(new() { PagesCount = 1 })
    {
        if (!Design.IsDesignMode)
            Source.Update();

        AddCityToMyRegionsCommand = ReactiveCommand.Create(Source.AddCityToRegions, CRUSCityControlViewModelSource.GlobalContainer.IsSelectedItemNotNull);

        RemoveCityFromMyRegionsCommand = ReactiveCommand.Create<RegionMonitoring>(Source.RemoveCityFromRegion);

        UpdateListCommand = ReactiveCommand.Create(Source.Update);

        AcceptChanges = ReactiveCommand.Create(Source.AcceptChanges);
    }
}
