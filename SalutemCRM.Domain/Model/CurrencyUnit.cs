using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class CurrencyUnit : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    public object Clone() { return this.MemberwiseClone(); }
}