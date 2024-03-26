using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SalutemCRM.Domain.Modell;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class User : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _userRoleForeignKey;

    [NotMapped]
    [ObservableProperty]
    private bool _isActive;

    [NotMapped]
    [ObservableProperty]
    private string _login = null!;

    [NotMapped]
    [ObservableProperty]
    private string _passwordMD5 = null!;

    [NotMapped]
    [ObservableProperty]
    private string _firstName = null!;

    [NotMapped]
    [ObservableProperty]
    private string _lastName = null!;



    [NotMapped]
    [ObservableProperty]
    private UserRole? _userRole;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<ManufactureProcess> _manufactureProcesses = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<OfficeOrder> _officeOrders = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<WarehouseOrder> _warehouseOrders = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<CustomerServiceOrder> _customerServiceOrders = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<CustomerService> _customerServices = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _materialsFlow = new();
}
