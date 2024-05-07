using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class Payment : ClonableObservableObject<Payment>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentValueBYN))]
    private string _currency = null!;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentValueBYN))]
    private double _unitToBYNConversion;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PaymentValueBYN))]
    private double _paymentValue;

    [NotMapped]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecordDate))]
    private DateTime _recordDT;



    [NotMapped]
    [ObservableProperty]
    private Order? _order;

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();
}