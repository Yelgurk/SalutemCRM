using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain.Model;

public partial class Payment : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _warehouseOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _officeOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _customerServiceOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;

    [NotMapped]
    [ObservableProperty]
    private string _currency = null!;

    [NotMapped]
    [ObservableProperty]
    private double _unitToBYNConversion;

    [NotMapped]
    [ObservableProperty]
    private double _paymentValue;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;



    [NotMapped]
    [ObservableProperty]
    private WarehouseOrder? _warehouseOrder;

    [NotMapped]
    [ObservableProperty]
    private OfficeOrder? _officeOrder;

    [NotMapped]
    [ObservableProperty]
    private CustomerServiceOrder? _customerServiceOrder;
}