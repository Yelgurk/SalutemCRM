using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class Order
{
    [NotMapped]
    public static Order Default => new()
    {
        OrderType = Order_Type.ManagerSale,
        PaymentAgreement = Payment_Status.PartiallyPaid,
        PaymentStatus = Payment_Status.Unpaid,
        TaskStatus = Task_Status.AwaitPayment
    };

    [NotMapped]
    public string OrderTypeDescription => OrderType switch
    {
        Order_Type.ManagerSale => "Продажа",
        Order_Type.CustomerService => "Сервис",
        Order_Type.WarehouseRestocking => "Закупка",
        _ => "Иное"
    };

    [NotMapped]
    public string PaymentEndpointPerson
    {
        get
        {
            if (Client is not null)
                return Client.Name;

            if (Vendor is not null)
                return Vendor.Name;

            return "[Салутем]";
        }
    }

    [NotMapped]
    public bool IsCustomerOrder => OrderType == Order_Type.WarehouseRestocking ? false : true;

    [NotMapped]
    public bool IsManagerSales => OrderType == Order_Type.ManagerSale ? true : false;

    [NotMapped]
    public bool IsWarehouseRestocking => OrderType == Order_Type.WarehouseRestocking ? true : false;

    [NotMapped]
    public bool IsServiceOrder => OrderType == Order_Type.CustomerService ? true : false;

    [NotMapped]
    public bool IsPaymentRequired => PaymentAgreement > Payment_Status.Unpaid ? true : false;

    [NotMapped]
    public bool IsPaymentPartial => PaymentAgreement == Payment_Status.PartiallyPaid ? true : false;

    [NotMapped]
    public double PriceTotalBYN => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion);

    [NotMapped]
    public double PricePaid => Payments.Select(x => x.PaymentValue).Sum();

    [NotMapped]
    public double PricePaidBYN => Payments.Select(x => x.PaymentValueBYN).Sum();

    [NotMapped]
    public double PricePaidPercantage => Math.Round(100.0 / PriceTotalBYN * PricePaidBYN, 2).Do(x => double.IsNaN(x) ? 0 : x);
}