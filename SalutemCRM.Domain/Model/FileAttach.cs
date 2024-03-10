using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class FileAttach : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _officeOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _warehouseOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _customerServiceOrderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;

    [NotMapped]
    [ObservableProperty]
    private string _fileName = null!;



    [NotMapped]
    [ObservableProperty]
    private OfficeOrder? _officeOrder;

    [NotMapped]
    [ObservableProperty]
    private WarehouseOrder? _warehouseOrder;

    [NotMapped]
    [ObservableProperty]
    private CustomerServiceOrder? _customerServiceOrder;
}