using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SalutemCRM.Domain.Model;

public partial class Vendor : ObservableObject
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
    [NotifyPropertyChangedFor(nameof(LastOrder))]
    [NotifyPropertyChangedFor(nameof(LastOrderDT))]
    private ObservableCollection<Order> _orders = new();

    [NotMapped]
    public Order? LastOrder => this.Orders.OrderBy(o => o.RecordDT).FirstOrDefault();

    [NotMapped]
    public string LastOrderDT => LastOrder?.RecordDT.ToString("dd.MM.yyyy HH:mm:ss") ?? "{ нет }";

    public object Clone() { return this.MemberwiseClone(); }
}