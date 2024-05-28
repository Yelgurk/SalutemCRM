using Avalonia.Media.Imaging;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using ReactiveUI;
using SalutemCRM.Control;
using SalutemCRM.Database;
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
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class WarehouseReceiveMaterialsControlViewModelSource : ReactiveControlSource<WarehouseKeeperOrder>
{
    public object? QRBitmap => App.Host!.Services.GetService<QRCodeGeneratorService>()!.Generate(KbInputCode);

    public string ScannerModeDescription => $"Сканнер | Режим удаления по коду: {(IsScannerModeRemove ? "ВКЛ" : "ВЫКЛ")}";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ScannerModeDescription))]
    private bool _isScannerModeRemove = false;

    [ObservableProperty]
    private bool _isAllInfoFullFilled = false;

    [ObservableProperty]
    private bool _isOverlayBindToWarehouseItem = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(QRBitmap))]
    private string _kbInputCode = "";

    [ObservableProperty]
    private WarehouseSupply? _selectedSupply;

    [ObservableProperty]
    private ObservableCollection<WarehouseSupply> _scannedCollection = new();

    public WarehouseReceiveMaterialsControlViewModelSource() => SelectedItemChangedTrigger += _newSelected =>
        ScannedCollection
        .Do(x => HideAllOverlays())
        .Do(x => x.Clear())
        .Do(x => SelectedItem!.MaterialsIn.DoForEach(s => x.Add(s.Clone())));

    public void HideAllOverlays() => false
        .Do(x => IsOverlayBindToWarehouseItem = x);

    public bool CheckIsAllInfoFullFilled() => IsAllInfoFullFilled = ScannedCollection.All(x => x.IsAllScanned);

    public void SaveScannedMaterial(string qrCode)
    {
        if (!IsOverlayBindToWarehouseItem)
        {
            if (SelectedSupply is null)
                SelectedSupply = ScannedCollection.SingleOrDefault(x => x.VendorCode == qrCode);

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
        }
    }

    public void RemoveScannedMaterial(string qrCode) => ScannedCollection
        .DoIf(x => { }, x => !IsOverlayBindToWarehouseItem)?
        .DoIf(x =>
        {
            ScannedCollection.Where(s => s.ScannedQrCodes.Any(z => z == qrCode)).Last()
                .Do(s => s.ScannedQrCodes.Remove(s.ScannedQrCodes.Where(z => z == qrCode).Last()))
                .Do(s => --s!.ScannedCount);
        }, x => ScannedCollection.Where(s => s.ScannedQrCodes.Any(z => z == qrCode)).Count() > 0);

    public void ScannerCallback(string qrCode)
    {
        if (IsScannerModeRemove)
            RemoveScannedMaterial(qrCode);
        else
            SaveScannedMaterial(qrCode);
    }

    public void ShowOverlayBindToWarehouseItem(WarehouseSupply _bindTarget)
    {
        SelectedSupply = _bindTarget;
        IsOverlayBindToWarehouseItem = true;
    }

    public void AcceptBindToWarehouseItem()
    {
        SelectedSupply!.WarehouseItem = CRUSWarehouseItemControlViewModelSource.GlobalContainer.SelectedItem!.Clone();
        HideAllOverlays();
    }

    public void AcceptSuccessfullyDeliveryToStock()
    {
        if (CheckIsAllInfoFullFilled())
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            {
                ScannedCollection.DoForEach(x => db.WarehouseSupplying.Add(new()
                {

                }));
            }






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
    }

    public void GoBack()
    {
        HideAllOverlays();
        ScannedCollection.Clear();
        NavigationViewModelSource.SetRegisteredWindowContent<WarehouseKeeperOrders>();
    }
}

public class WarehouseReceiveMaterialsControlViewModel : ViewModelBase<WarehouseKeeperOrder, WarehouseReceiveMaterialsControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? AcceptSuccessfullyDeliveryToStockCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? HideOverlaysCommand { get; protected set; }

    public ReactiveCommand<WarehouseSupply, Unit>? ShowOverlayBindToWarehouseItemCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AcceptBindToWarehouseItemCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddQrCodeByInputStrCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? RemoveQrCodeByInputStrCommand { get; protected set; }

    public WarehouseReceiveMaterialsControlViewModel() : base(new() { PagesCount = 1 })
    {
        QRCodeScanService.Init();
        QRCodeScanService.QRCodeScannedEvent += qrCode =>
        {
            if (NavigationViewModelSource.IsCurrentScreen<WarehouseReceiveMaterialsControl>())
                Source.ScannerCallback(qrCode);
        };

        Observable.Interval(TimeSpan.FromMilliseconds(500)).Subscribe(_ => Source.CheckIsAllInfoFullFilled());

        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.KbInputCode,    
            x => x.Source.KbInputCode,
            (str1, str2) => str1.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(Source.GoBack);

        AcceptSuccessfullyDeliveryToStockCommand = ReactiveCommand.Create(Source.AcceptSuccessfullyDeliveryToStock);

        HideOverlaysCommand = ReactiveCommand.Create(Source.HideAllOverlays);

        ShowOverlayBindToWarehouseItemCommand = ReactiveCommand.Create<WarehouseSupply>(Source.ShowOverlayBindToWarehouseItem);

        AcceptBindToWarehouseItemCommand = ReactiveCommand.Create(Source.AcceptBindToWarehouseItem, CRUSWarehouseItemControlViewModelSource.GlobalContainer.IsSelectedItemNotNull);

        AddQrCodeByInputStrCommand = ReactiveCommand.Create(() => Source.SaveScannedMaterial(Source.KbInputCode), IfSearchStrNotNull);

        RemoveQrCodeByInputStrCommand = ReactiveCommand.Create(() => Source.RemoveScannedMaterial(Source.KbInputCode), IfSearchStrNotNull);
    }
}
