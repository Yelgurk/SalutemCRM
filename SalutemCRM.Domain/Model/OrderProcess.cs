using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class OrderProcess : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _employeeForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _manufactureForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _orderDutyForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Task_Status _taskStatus;

    [NotMapped]
    [ObservableProperty]
    private int _queue;

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
    private Manufacture? _manufacture;

    [NotMapped]
    [ObservableProperty]
    private Order? _order;

    [NotMapped]
    [ObservableProperty]
    private OrderDuty? _orderDuty;

    public object Clone() { return this.MemberwiseClone(); }
}