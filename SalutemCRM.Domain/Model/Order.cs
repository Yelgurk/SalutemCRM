using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class Order : ClonableObservableObject<Order>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _employeeForeignkey;

    [NotMapped]
    [ObservableProperty]
    private int? _clientForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _vendorForeignKey;



    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCustomerOrder))]
    [NotifyPropertyChangedFor(nameof(IsServiceOrder))]
    private Order_Type _orderType;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPaymentRequired))]
    [NotifyPropertyChangedFor(nameof(IsPaymentPartial))]
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
    private User? _employee;

    [NotMapped]
    [ObservableProperty]
    private Client? _client;

    [NotMapped]
    [ObservableProperty]
    private Vendor? _vendor;

    

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
    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _warehouseSupplies = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Manufacture> _manufactures = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<OrderProcess> _orderProcesses = new();
}