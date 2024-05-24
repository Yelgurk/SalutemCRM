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
    [NotifyPropertyChangedFor(nameof(IsManagerSales))]
    [NotifyPropertyChangedFor(nameof(IsWarehouseRestocking))]
    [NotifyPropertyChangedFor(nameof(IsServiceOrder))]
    [NotifyPropertyChangedFor(nameof(OrderTypeDescription))]
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
    [NotifyPropertyChangedFor(nameof(TaskStatusTypeDescription))]
    [NotifyPropertyChangedFor(nameof(IsOrderManufactureExecuted))]
    [NotifyPropertyChangedFor(nameof(IsOrderAwaitManufacture))]
    [NotifyPropertyChangedFor(nameof(IsOrderFinished))]
    private Task_Status _taskStatus;


    [NotMapped]
    [ObservableProperty]
    private string _additionalInfo = null!;

    [NotMapped]
    [ObservableProperty]
    private int _daysOnHold = 10;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    private string _currency = "BYN";

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    private double _unitToBYNConversion = 1.00;

    [NotMapped]
    [ObservableProperty]
    private double _priceRequired;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriceTotalBYN))]
    private double _priceTotal;



    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecordDate))]
    private DateTime _recordDT;

    [NotMapped]
    [ObservableProperty]
    private DateTime? _deadlineDT;

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
    [NotifyPropertyChangedFor(nameof(PaymentEndpointPerson))]
    private Client? _client;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentEndpointPerson))]
    private Vendor? _vendor;

    

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PricePaid))]
    [NotifyPropertyChangedFor(nameof(PricePaidBYN))]
    [NotifyPropertyChangedFor(nameof(PricePaidPercantage))]
    private ObservableCollection<Payment> _payments = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskCompletedPercentage))]
    private ObservableCollection<WarehouseSupply> _warehouseSupplies = new();

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaskCompletedPercentage))]
    private ObservableCollection<Manufacture> _manufactures = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<OrderProcess> _orderProcesses = new();
}