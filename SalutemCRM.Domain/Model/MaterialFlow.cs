using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain.Model;

public partial class MaterialFlow : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _employeeForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _warehouseSupplyForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _manufactureForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string _additionalInfo = null!;

    [NotMapped]
    [ObservableProperty]
    private double _count;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;



    [NotMapped]
    [ObservableProperty]
    private User? _employee;

    [NotMapped]
    [ObservableProperty]
    private WarehouseSupply? _warehouseSupply;

    [NotMapped]
    [ObservableProperty]
    private Manufacture? _manufacture;

    [NotMapped]
    [ObservableProperty]
    private Order? _order;
}
