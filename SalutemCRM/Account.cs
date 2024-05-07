using CommunityToolkit.Mvvm.ComponentModel;
using SalutemCRM.Domain.Model;
using SalutemCRM.Domain.Modell;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM;

public partial class Account : ObservableObject
{
    public static Account Current { get; } = new();

    [ObservableProperty]
    private User _user = User.Storekeeper;

    public static void SetAccount(User SignIn) => Current.User = SignIn;

    partial void OnUserChanged(User value)
    {
        OnPropertyChanged(nameof(IsRootOrBossUser));
        OnPropertyChanged(nameof(IsBookkeeperUser));
        OnPropertyChanged(nameof(IsSeniorSalesManagerUser));
        OnPropertyChanged(nameof(IsSalesManagerUser));
        OnPropertyChanged(nameof(IsManufactureManagerUser));
        OnPropertyChanged(nameof(IsConstrEngineer));
        OnPropertyChanged(nameof(IsManufactureEmployeeUser));
        OnPropertyChanged(nameof(IsStorekeeperUser));
        OnPropertyChanged(nameof(IsMoneyStateInfoVisible));
        OnPropertyChanged(nameof(IsStockCountInfoVisible));
    }

    public bool IsRootOrBossUser => Current.User.Permission == User_Permission.Boss;

    public bool IsBookkeeperUser => Current.User.Permission == User_Permission.Bookkeeper;

    public bool IsSeniorSalesManagerUser => Current.User.Permission == User_Permission.SeniorSalesManager;

    public bool IsSalesManagerUser => Current.User.Permission == User_Permission.SalesManager;

    public bool IsManufactureManagerUser => Current.User.Permission == User_Permission.ManufactureManager;

    public bool IsConstrEngineer => Current.User.Permission == User_Permission.ConstrEngineer;

    public bool IsManufactureEmployeeUser => Current.User.Permission == User_Permission.ManufactureEmployee;

    public bool IsStorekeeperUser => Current.User.Permission == User_Permission.Storekeeper;

    public bool IsMoneyStateInfoVisible => IsRootOrBossUser;

    public bool IsStockCountInfoVisible => IsRootOrBossUser | IsManufactureManagerUser;
}
