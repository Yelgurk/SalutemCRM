using CommunityToolkit.Mvvm.ComponentModel;
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
    private WarehouseItem? _refWarehouseItem;

    [ObservableProperty]
    private WarehouseSupply? _refWarehouseSupply;

    [ObservableProperty]
    private WarehouseItem? _warehouseItem;

    [ObservableProperty]
    private WarehouseSupply? warehouseSupply;

    public ScannedMaterial(string ScannedCode, WarehouseItem? RefItem) : this(ScannedCode, RefItem, null) { }

    public ScannedMaterial(string ScannedCode, WarehouseSupply? RefSupply) : this(ScannedCode, null, RefSupply) { }

    public ScannedMaterial(string ScannedCode, WarehouseItem? RefItem, WarehouseSupply? RefSupply) : this(ScannedCode)
    {
        _refWarehouseItem = RefItem;
        _refWarehouseSupply = RefSupply;
    }

    public ScannedMaterial(string ScannedCode)
    {
        _scannedCode = ScannedCode;

        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
        {
            WarehouseItem = db.WarehouseItems.SingleOrDefault(x => x.InnerCode == _scannedCode);
            WarehouseSupply = db.WarehouseSupplying.SingleOrDefault(x => x.VendorCode == _scannedCode);
        }

        IsNewMaterial = WarehouseItem is null && WarehouseSupply is null;
    }
}