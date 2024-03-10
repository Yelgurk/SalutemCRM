using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class CustomerServiceOrder : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _stockManagerForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _manufactureForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Payment_Status _paymentAgreement;

    [NotMapped]
    [ObservableProperty]
    private Payment_Status _paymentStatus;

    [NotMapped]
    [ObservableProperty]
    private Task_Status _taskStatus;

    [NotMapped]
    [ObservableProperty]
    private string _additionalInfo = null!;

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
    private DateTime _deadlineDT;

    [NotMapped]
    [ObservableProperty]
    private DateTime? _startedDT;

    [NotMapped]
    [ObservableProperty]
    private DateTime? _completedDT;



    [NotMapped]
    [ObservableProperty]
    private User? _stockManager;

    [NotMapped]
    [ObservableProperty]
    private Manufacture? _manufacture;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Payment> _payments = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<CustomerService> _customerServices = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();



    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}
