using ReactiveUI;
using SalutemCRM.Control;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.Database;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SalutemCRM.ViewModels;

/*
 
if (x.OrderType == Order_Type.CustomerService)
                    {
                        foreach (var item in OrderServiceItem)
                            db.MaterialFlow.Add(new()
                            {
                                ReturnedForeignKey = null,
                                EmployeeForeignKey = x.EmployeeForeignkey,
                                WarehouseItemForeignKey = item.Id,
                                WarehouseSupplyForeignKey = null,
                                ManufactureForeignKey = null,
                                OrderForeignKey = OrderID,
                                AdditionalInfo = x.AdditionalInfo ?? $"[счёт #{OrderID}, {Account.Current.User.FullNameWithLogin} : нет дополнительной информации]",
                                CountReservedFromStock = item.OrderBuilder_Count,
                                CountProvidedFromStock = 0,
                                CountReturnedToStock = 0,
                                DeliveryStatus = Delivery_Status.NotDelivered,
                                RecordDT = RecordDT
                            });
                    }
материалы на затраты установлены при оформлении заявки
показать заявку когда будет оплата

else if (x.OrderType == Order_Type.ManagerSale)
материалы на использовании в производстве устанавливаются после расстановки задач боссом производства
показать заявку, когда будет этап производства на сотруднике-кладовщике на оборудование
каждая заявка на оборудование отдельно, а не на весь счёт
 
*/

/*
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
       */

public partial class WarehouseProvideMaterialsControlViewModelSource : ReactiveControlSource<WarehouseKeeperOrder>
{
    public object? QRBitmap => App.Host!.Services.GetService<QRCodeGeneratorService>()!.Generate(KbInputCode);

    public string ScannerModeDescription => $"Сканнер | Режим удаления по коду: {(IsScannerModeRemove ? "ВКЛ" : "ВЫКЛ")}";

    [ObservableProperty]
    private string _blankHeader = "";

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
    private ObservableCollection<MaterialFlow> _providingCollection = new();

    [ObservableProperty]
    private MaterialFlow? _selectedMaterial;

    public WarehouseProvideMaterialsControlViewModelSource() => SelectedItemChangedTrigger += _newSelected =>
        ProvidingCollection
        .Do(x => HideAllOverlays())
        .Do(x => x.Clear())
        .Do(x => SelectedItem!.MaterialsOut.DoForEach(s => x.Add(s.Clone().DoInst(f => f.WarehouseItem = s.WarehouseItem!.Clone()))));

    public void HideAllOverlays() => false
        .Do(x => IsOverlayBindToWarehouseItem = x);

    public bool CheckIsAllInfoFullFilled() => IsAllInfoFullFilled =
        ProvidingCollection
        .All(x => x.CountReservedFromStock == x.ScannedMaterialsFromWarehouse.Select(s => s.ScannedCount).Sum() || x.CountReservedFromStock == -1);

    public void SaveScannedMaterial(string qrCode) => SaveScannedMaterial(new ScannedMaterial(qrCode));
    public void SaveScannedMaterial(ScannedMaterial _scanResult)
    {
        if (!IsOverlayBindToWarehouseItem)
        {
            if (ProvidingCollection.SingleOrDefault(x => x.WarehouseItem!.Id == (_scanResult.WarehouseSupply?.WarehouseItem.Id ?? -1)) is MaterialFlow _exists && _exists is not null)
            {
                SelectedMaterial = _exists;

                if (SelectedMaterial.ScannedMaterialsFromWarehouse.SingleOrDefault(x => x.Id == _scanResult.WarehouseSupply!.Id) is WarehouseSupply _found && _found is not null)
                    ++_found!.ScannedCount;
                else
                    SelectedMaterial.ScannedMaterialsFromWarehouse.Add(_scanResult.WarehouseSupply!.Clone().DoInst(x => x.ScannedCount = 1));
            }
            else if (_scanResult.WarehouseSupply is not null)
            {
                ProvidingCollection.Add(new MaterialFlow()
                {
                    Id = -1,
                    ReturnedForeignKey = null,
                    EmployeeForeignKey = Account.Current.User.Id,
                    WarehouseItemForeignKey = _scanResult.WarehouseSupply.WarehouseItem.Id,
                    WarehouseItem = _scanResult.WarehouseSupply.WarehouseItem.Clone(),
                    WarehouseSupplyForeignKey = _scanResult.WarehouseSupply.Id,
                    WarehouseSupply = _scanResult.WarehouseSupply.Clone(),
                    ManufactureForeignKey = SelectedItem!.OrderType == Order_Type.ManagerSale ? SelectedItem.Manufacture!.Id : null,
                    OrderForeignKey = SelectedItem!.OrderType == Order_Type.CustomerService ? SelectedItem.Order!.Id : null,
                    AdditionalInfo = $"" +
                    $"[{(SelectedItem!.OrderType == Order_Type.ManagerSale ? $"изделие #{SelectedItem.Manufacture!.Code}" : "")}" +
                    $"[{(SelectedItem!.OrderType == Order_Type.CustomerService ? $"счёт#{SelectedItem.Order!.Id}" : "")}" +
                    $", {Account.Current.User.FullNameWithLogin} : экстра-добавлен на этапе выдачи со склада]" +
                    $"",
                    CountReservedFromStock = -1,
                    CountProvidedFromStock = 0,
                    CountReturnedToStock = 0,
                    DeliveryStatus = Delivery_Status.NotDelivered,
                    RecordDT = DateTime.Now
                }.Do(x => x.ScannedMaterialsFromWarehouse.Add(_scanResult.WarehouseSupply!.Clone().DoInst(s => s.ScannedCount = 1))));
            }
            /*
            else if (_scanResult.WarehouseItem is not null && _scanResult.WarehouseItem.WarehouseSupplying.Count() > 0)
            {
            }
            */
        }
    }

    public void RemoveScannedMaterial(string qrCode) => RemoveScannedMaterial(new ScannedMaterial(qrCode));
    public void RemoveScannedMaterial(ScannedMaterial _scanResult) => ProvidingCollection
        .DoIf(x => { }, x => !IsOverlayBindToWarehouseItem)?
        .Do(x => ProvidingCollection.SingleOrDefault(s => s.WarehouseItem!.Id == (_scanResult.WarehouseSupply?.WarehouseItem.Id ?? -1)))?
        .Do(x =>
        {
            if (x.ScannedMaterialsFromWarehouse.SingleOrDefault(s => s.Id == _scanResult.WarehouseSupply!.Id) is WarehouseSupply _match && _match is not null)
            {
                --_match.ScannedCount;

                if (_match.ScannedCount <= 0)
                    x.ScannedMaterialsFromWarehouse.Remove(_match);
            }
        })
        .DoIf(x => ProvidingCollection.Remove(x), x => x.Id == -1 && x.ScannedMaterialsFromWarehouse.Select(s => s.ScannedCount).Sum() <= 0);

    public void ScannerCallback(string qrCode)
    {
        if (IsScannerModeRemove)
            RemoveScannedMaterial(qrCode);
        else
            SaveScannedMaterial(qrCode);
    }

    public void AcceptProvidingFromStock()
    {
        if (CheckIsAllInfoFullFilled())
        {
            Debug.WriteLine("ПОДТВЕРЖДАЮ выдачу материала для производства/<сервисного обслуживания>");
            GoBack();

            /*
            using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            {
                var _edited = db.Orders.Single(x => x.Id == SelectedItem!.Order!.Id)
                    .DoInst(x => x.TaskStatus = ScannedCollection.Select(s => s.OrderCount - s.ScannedCount).Sum() == 0 ? Task_Status.Finished : Task_Status.Execution);

                ScannedCollection
                .Where(x => x.ScannedCount > 0)
                .DoForEach(x => x.GetScanResult.DoForEach(s => db.WarehouseSupplying.Add(new()
                {
                    WarehouseItemForeignKey = x.WarehouseItem.Id,
                    OrderForeignKey = x.OrderForeignKey,
                    DeliveryStatus = Delivery_Status.FullyDelivered,
                    VendorName = x.VendorName,
                    VendorCode = s.code,
                    Currency = x.Currency,
                    UnitToBYNConversion = x.UnitToBYNConversion,
                    PriceTotal = x.PriceTotal / x.OrderCount * s.totalCount,
                    OrderCount = s.totalCount,
                    InStockCount = s.totalCount,
                    RecordDT = x.RecordDT
                })));

                if (IsPartialReceivingAvailable)
                    ScannedCollection
                    .Where(x => x.OrderCount - x.ScannedCount > 0)
                    .DoForEach(x => db.WarehouseSupplying.Add(new()
                    {
                        OrderForeignKey = x.OrderForeignKey,
                        DeliveryStatus = Delivery_Status.NotDelivered,
                        VendorName = x.VendorName,
                        Currency = x.Currency,
                        UnitToBYNConversion = x.UnitToBYNConversion,
                        PriceTotal = x.PriceTotal / x.OrderCount * (x.OrderCount - x.ScannedCount),
                        OrderCount = x.OrderCount - x.ScannedCount,
                        InStockCount = 0,
                        RecordDT = x.RecordDT
                    }));

                (from v1 in db.WarehouseSupplying.AsEnumerable()
                 join v2 in ScannedCollection on v1.Id equals v2.Id
                 select v1)
                .DoForEach(x => db.WarehouseSupplying.Remove(x));

                db.SaveChanges();
            }

            GoBack();
            */
        }
    }

    public void ShowWarehouseInfo()
    {
        IsOverlayBindToWarehouseItem = true;
    }

    public void GoBack()
    {
        HideAllOverlays();
        ProvidingCollection.Clear();
        NavigationViewModelSource.SetRegisteredWindowContent<WarehouseKeeperOrders>();
        App.Host!.Services.GetService<WarehouseKeeperOrdersViewModel>()!.Source.Update();
    }
}

public class WarehouseProvideMaterialsControlViewModel : ViewModelBase<WarehouseKeeperOrder, WarehouseProvideMaterialsControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? AcceptProvidingFromStockCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? ShowWarehouseInfoCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? HideOverlaysCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? AddQrCodeByInputStrCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit>? RemoveQrCodeByInputStrCommand { get; protected set; }

    public WarehouseProvideMaterialsControlViewModel() : base(new() { PagesCount = 1 })
    {
        QRCodeScanService.Init();
        QRCodeScanService.QRCodeScannedEvent += qrCode =>
        {
            if (NavigationViewModelSource.IsCurrentScreen<WarehouseProvideMaterialsControl>())
                Source.ScannerCallback(qrCode);
        };

        Observable.Interval(TimeSpan.FromMilliseconds(500)).Subscribe(_ => Source.CheckIsAllInfoFullFilled());

        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.KbInputCode,
            x => x.Source.KbInputCode,
            (str1, str2) => str1.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(Source.GoBack);

        AcceptProvidingFromStockCommand = ReactiveCommand.Create(Source.AcceptProvidingFromStock);

        ShowWarehouseInfoCommand = ReactiveCommand.Create(Source.ShowWarehouseInfo);

        HideOverlaysCommand = ReactiveCommand.Create(Source.HideAllOverlays);

        AddQrCodeByInputStrCommand = ReactiveCommand.Create(() => Source.SaveScannedMaterial(Source.KbInputCode), IfSearchStrNotNull);

        RemoveQrCodeByInputStrCommand = ReactiveCommand.Create(() => Source.RemoveScannedMaterial(Source.KbInputCode), IfSearchStrNotNull);
    }
}
