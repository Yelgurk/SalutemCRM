using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class Manufacture : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Delivery_Status _deliveryStatus;

    [NotMapped]
    [ObservableProperty]
    private Task_Status _taskStatus;

    [NotMapped]
    [ObservableProperty]
    private string _code = null!;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    [NotMapped]
    [ObservableProperty]
    private string _model = null!;

    [NotMapped]
    [ObservableProperty]
    private string _additionalInfo = null!;

    [NotMapped]
    [ObservableProperty]
    private DateTime? _shipmentDT;




    [NotMapped]
    [ObservableProperty]
    private Order? _order;




    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialFlows = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<OrderProcess> _orderProcesses = new();

    public object Clone() { return this.MemberwiseClone(); }
}