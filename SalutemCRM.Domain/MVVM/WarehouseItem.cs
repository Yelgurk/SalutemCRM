﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    private object? _qRBitmap;

    [NotMapped]
    public object? QRBitmap
    {
        get => _qRBitmap;
        set => SetProperty(ref _qRBitmap, value);
    }
}