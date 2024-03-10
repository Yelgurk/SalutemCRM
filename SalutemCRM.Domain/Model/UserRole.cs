using CommunityToolkit.Mvvm.ComponentModel;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Modell;

public partial class UserRole : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<User> _users = new();
}