using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class Manufacture
{
    [NotMapped]
    public ObservableCollection<WarehouseItem> StartupItemsCollection { get; } = new();

    [NotMapped]
    public ObservableCollection<WarehouseSupply> EditedMaterialsUsedCollection { get; } = new();

    [NotMapped]
    public double CompletedPercentage => OrderProcesses
        .DoIf(x => { }, x => x.Count > 0)?
        .Do(x => Extensions.PercentageCalc(
            x.Count * Task_Status.Finished.Cast<int>(),
            x.Select(s => s.TaskStatus.Cast<int>())
            .DoForEach(s => s = s > Task_Status.Finished.Cast<int>() ? 0 : s)
            .Sum()
        )) ?? 0.0;
}