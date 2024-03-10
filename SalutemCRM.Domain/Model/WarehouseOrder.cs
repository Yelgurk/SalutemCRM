using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseOrder : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _storekeeperForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _vendorForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Payment_Status _paymentAgreement;

    [NotMapped]
    [ObservableProperty]
    private Payment_Status _paymentStatus;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    private string _currency = null!;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    private double _unitToBYNConversion;

    [NotMapped]
    [ObservableProperty]
    private double _priceRequired;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    private double _priceTotal;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;

    [NotMapped]
    [ObservableProperty]
    private DateTime _shipmentDeadlineDT;

    [NotMapped]
    [ObservableProperty]
    private DateTime? _receivedDT;



    [NotMapped]
    [ObservableProperty]
    private User? _storekeeper;

    [NotMapped]
    [ObservableProperty]
    private Vendor? _vendor;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Payment> _payments = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _warehouseSupplies = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();



    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}
