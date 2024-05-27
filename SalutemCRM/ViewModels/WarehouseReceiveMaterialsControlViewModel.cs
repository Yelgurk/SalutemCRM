using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class WarehouseReceiveMaterialsControlViewModelSource : ReactiveControlSource<WarehouseSupply>
{
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

public class WarehouseReceiveMaterialsControlViewModel : ViewModelBase<WarehouseSupply, WarehouseReceiveMaterialsControlViewModelSource>
{
    public WarehouseReceiveMaterialsControlViewModel() : base(new() { PagesCount = 1 })
    {
    }
}
