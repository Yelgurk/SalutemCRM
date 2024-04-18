
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class Client
{
    [NotMapped]
    public Order? LastOrder => this.Orders.OrderBy(o => o.RecordDT).FirstOrDefault();

    [NotMapped]
    public string LastOrderDT => LastOrder?.RecordDT.ToString("dd.MM.yyyy HH:mm:ss") ?? "{ нет }";
}