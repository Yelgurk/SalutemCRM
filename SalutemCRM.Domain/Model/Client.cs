
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class Client : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    [NotMapped]
    [ObservableProperty]
    private string _address = null!;

    [NotMapped]
    [ObservableProperty]
    private string? _additionalInfo;

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Order> _orders = new();

    public object Clone() { return this.MemberwiseClone(); }
}