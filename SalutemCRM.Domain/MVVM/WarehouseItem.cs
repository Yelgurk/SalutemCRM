using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class WarehouseItem
{
    [NotMapped]
    public double TotalInStockCount { get => WarehouseSupplying.Select(x => x.InStockCount).Sum(); }

    [NotMapped]
    public Stock_Status CountInStockState { get => TotalInStockCount switch
        {
            var x when x >= CountRequired * 1.5         => Stock_Status.Enough,
            var x when x >= CountRequired               => Stock_Status.CloseToLimit,
            var x when x >= 1.0 && x < CountRequired    => Stock_Status.NotEnough,
            _                                           => Stock_Status.ZeroWarning
        };
    }

    [NotMapped]
    public string CountInStockStateToString =>
        (int)CountInStockState < EnumToString.StockStatusToString.Count ?
        EnumToString.StockStatusToString[(int)CountInStockState] :
        "неизвестная ошибка";

    [NotMapped]
    public double BYNPriceForSingleInStock { get => WarehouseSupplying.Select(x => x.InStockPriceTotalBYN).Sum() / TotalInStockCount; }

    [NotMapped]
    public double BYNPriceForTotalInStock { get => WarehouseSupplying.Select(x => x.InStockPriceTotalBYN).Sum(); }

    [NotMapped]
    private object? _qRBitmap;

    [NotMapped]
    public object? QRBitmap
    {
        get => _qRBitmap;
        set => SetProperty(ref _qRBitmap, value);
    }

    partial void OnCountRequiredChanged(double value) => CountRequired = Double.IsNaN(value) ? 0 : value;
}