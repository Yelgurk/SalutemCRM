using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class Vendor : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    [NotMapped]
    [ObservableProperty]
    private string _address = null!;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<WarehouseOrder> _warehouseOrders = new();
}