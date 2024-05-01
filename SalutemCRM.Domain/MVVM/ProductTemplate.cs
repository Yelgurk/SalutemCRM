using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class ProductTemplate
{
    [NotMapped]
    private int _orderBasketCount = 1;

    [NotMapped]
    public int OrderBasketCount
    {
        get => _orderBasketCount;
        set
        {
            if (value > 0)
            {
                _orderBasketCount = value;
                OnPropertyChanged(nameof(OrderBasketCount));
            }
        }
    }
}