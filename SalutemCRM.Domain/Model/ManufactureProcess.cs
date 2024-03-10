using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class ManufactureProcess : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _employeeForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _manufactureForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _manufactureDutyForeignKey;

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
    private ManufacturerDuty? _manufacturerDuty;
}