using ReactiveUI;
using SalutemCRM.Control;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

public class WarehouseProvideMaterialsControlViewModelSource : ReactiveControlSource<WarehouseKeeperOrder>
{
    public void SaveScannedMaterial(string qrCode)
    {
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
    }
}

public class WarehouseProvideMaterialsControlViewModel : ViewModelBase<WarehouseKeeperOrder, WarehouseProvideMaterialsControlViewModelSource>
{
    public WarehouseProvideMaterialsControlViewModel() : base(new() { PagesCount = 1 })
    {
        QRCodeScanService.Init();
        QRCodeScanService.QRCodeScannedEvent += qrCode =>
        {
            if (NavigationViewModelSource.IsCurrentScreen<WarehouseProvideMaterialsControl>())
                Source.SaveScannedMaterial(qrCode);
        };

        /*
        IfNewFilled = this.WhenAnyValue(
            x => x.Source.IsAllItemsScanned,
            x => x.Source.IsAllItemsScanned,
            (b1, b2) => b1
        );
        */
    }
}
