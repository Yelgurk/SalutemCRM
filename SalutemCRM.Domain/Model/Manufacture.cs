using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class Manufacture : ClonableObservableObject<Manufacture>
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
    private bool _haveSerialNumber;

    [NotMapped]
    [ObservableProperty]
    private string? _code;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    /* If (HaveSerialNumber) then Count can be only 1 */
    [NotMapped]
    [ObservableProperty]
    private int _count = 1;

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
}