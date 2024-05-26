using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class MaterialFlow
{
    [NotMapped]
    private double _countWillBeReturnedToStock = 0;

    [NotMapped]
    public double CountWillBeReturnedToStock
    {
        get => _countWillBeReturnedToStock <= 0 ? (_countWillBeReturnedToStock = CountReservedFromStock) : _countWillBeReturnedToStock;
        set => _countWillBeReturnedToStock = value;
    }
}
