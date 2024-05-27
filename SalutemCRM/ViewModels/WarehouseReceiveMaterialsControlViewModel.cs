using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Microsoft.VisualBasic;
using ReactiveUI;
using SalutemCRM.Control;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

/*
 * 
if (x.OrderType == Order_Type.WarehouseRestocking)
                    {
                        foreach (var item in OrderWarehouseSupplies)
                            db.WarehouseSupplying.Add(new()
                            {
                                WarehouseItemForeignKey = null,
                                OrderForeignKey = OrderID,
                                DeliveryStatus = Delivery_Status.NotDelivered,
                                VendorName = item.VendorName,
                                VendorCode = null,
                                Currency = item.Currency,
                                UnitToBYNConversion = item.OrderBuilder_ToBYNConv,
                                PriceTotal = item.OrderBuilder_PriceTotal,
                                OrderCount = item.OrderBuilder_Count,
                                InStockCount = 0,
                                RecordDT = RecordDT
                            });
                    }
материалы на закупку установлены при оформлении заявки
показать заявку, когда будет оплата

*/

public partial class WarehouseReceiveMaterialsControlViewModelSource : ReactiveControlSource<WarehouseKeeperOrder>
{
    [ObservableProperty]
    private WarehouseSupply? _selectedSupply;

    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _scannedCollection = new();

    [ObservableProperty]
    private bool _isAllItemsScanned = false;

    public WarehouseReceiveMaterialsControlViewModelSource() => SelectedItemChangedTrigger += _newSelected => { /* тут логика когда выбран новый WarehouseKeeperOrder в главном контроле */ };

    public void SaveScannedMaterial(string qrCode)
    {
        if (SelectedSupply is not null && ScannedCollection.SingleOrDefault(x => x.VendorName == SelectedSupply.VendorName) is WarehouseSupply _match && _match != null)
        {
            _match.ScannedCount += 1;
            _match.ScannedQrCodes.Add(qrCode);
        }
        else if (SelectedSupply is not null)
            ScannedCollection.Add(
                SelectedSupply
                .Clone()
                .Do(x => x.ScannedQrCodes.Add(qrCode))
                .DoInst(x => x.ScannedCount = 1)
            );
        IsAllItemsScanned =
            ScannedCollection.Count == SelectedItem!.MaterialsIn.Count &&
            ScannedCollection
            .GroupBy(x => x.VendorName)
            .Select(x => new { VendorName = x.Key, ScannedCount = x.Sum(x => x.ScannedCount) })
            .Join(SelectedItem!.MaterialsIn, x => x.VendorName, y => y.VendorName, (x, y) => x.ScannedCount == y.OrderCount)
            .All(result => result);
    }

    public void AcceptSuccessfullyDeliveryToStock()
    {
        Debug.WriteLine("Да, материалы приняты на склад");
    }

    public void GoBack()
    {
        IsAllItemsScanned = false;
        ScannedCollection.Clear();
        NavigationViewModelSource.SetRegisteredWindowContent<WarehouseKeeperOrders>();
    }
}

public class WarehouseReceiveMaterialsControlViewModel : ViewModelBase<WarehouseKeeperOrder, WarehouseReceiveMaterialsControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? AcceptSuccessfullyDeliveryToStockCommand { get; protected set; }

    public WarehouseReceiveMaterialsControlViewModel() : base(new() { PagesCount = 1 })
    {
        QRCodeScanService.Init();
        QRCodeScanService.QRCodeScannedEvent += qrCode =>
        {
            if (NavigationViewModelSource.IsCurrentScreen<WarehouseReceiveMaterialsControl>())
                Source.SaveScannedMaterial(qrCode);
        };

        IfNewFilled = this.WhenAnyValue(
            x => x.Source.IsAllItemsScanned,    
            x => x.Source.IsAllItemsScanned,
            (b1, b2) => b1
        );

        GoBackCommand = ReactiveCommand.Create(Source.GoBack);

        AcceptSuccessfullyDeliveryToStockCommand = ReactiveCommand.Create(Source.AcceptSuccessfullyDeliveryToStock, IfNewFilled);
    }
}
