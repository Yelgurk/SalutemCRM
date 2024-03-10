using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class OfficeOrder : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _managerForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _clientForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Order_Type _orderType;

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
    private DateTime? _shipmentDT;



    [NotMapped]
    [ObservableProperty]
    private User? _manager;

    [NotMapped]
    [ObservableProperty]
    private Client? _client;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Manufacture> _manufactures = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Payment> _payments = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();



    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}
