using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalutemCRM;

public partial class ScannedMaterial : ObservableObject
{
    [ObservableProperty]
    private string _scannedCode;

    [ObservableProperty]
    private bool _isNewMaterial;

    [ObservableProperty]
    private WarehouseItem? _warehouseItem;

    [ObservableProperty]
    private WarehouseSupply? warehouseSupply;

    public ScannedMaterial(string ScannedCode)
    {
        _scannedCode = ScannedCode;

        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
        {
            WarehouseItem = db.WarehouseItems
                .Include(x => x.WarehouseSupplying)
                .Where(x => x.WarehouseSupplying.Any(s => s.InStockCount > 0))
                .SingleOrDefault(x => x.InnerCode == _scannedCode);

            WarehouseSupply = db.WarehouseSupplying
                .Include(x => x.WarehouseItem)
                .Where(x => x.InStockCount > 0)
                .SingleOrDefault(x => x.VendorCode == _scannedCode);
        }

        IsNewMaterial = WarehouseItem is null && WarehouseSupply is null;
    }
}