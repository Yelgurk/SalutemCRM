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

    public bool IsRootOrBossUser => (Current.User.UserRole?.Name ?? "") == "Руководитель";

    public bool IsBookkeeperUser => (Current.User.UserRole?.Name ?? "") == "Бухгалтер";

    public bool IsSeniorSalesManagerUser => (Current.User.UserRole?.Name ?? "") == "Гл. менеджер";

    public bool IsSalesManagerUser => (Current.User.UserRole?.Name ?? "") == "Менеджер";

    public bool IsManufactureManagerUser => (Current.User.UserRole?.Name ?? "") == "Гл. производства";

    public bool IsConstrEngineer => (Current.User.UserRole?.Name ?? "") == "Конструктор";

    public bool IsManufactureEmployeeUser => (Current.User.UserRole?.Name ?? "") == "Производство";

    public bool IsStorekeeperUser => (Current.User.UserRole?.Name ?? "") == "Кладовщик";

    public bool IsMoneyStateInfoVisible => IsRootOrBossUser;

    public bool IsStockCountInfoVisible => IsRootOrBossUser | IsManufactureManagerUser;
}
