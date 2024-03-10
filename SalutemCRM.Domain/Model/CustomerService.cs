using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class CustomerService : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _employeeForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _customerServiceOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private Task_Status _taskStatus;

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
    private CustomerServiceOrder? _customerServiceOrder;
}
