using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public class WarehouseProvideMaterialsControlViewModelSource : ReactiveControlSource<MaterialFlow>
{
}

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

public class WarehouseProvideMaterialsControlViewModel : ViewModelBase<MaterialFlow, WarehouseProvideMaterialsControlViewModelSource>
{
    public WarehouseProvideMaterialsControlViewModel() : base(new() { PagesCount = 1 })
    {
    }
}
