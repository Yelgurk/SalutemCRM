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
    public bool IsCustomerOrder => OrderType == Order_Type.WarehouseRestocking ? false : true;

    [NotMapped]
    public bool IsServiceOrder => OrderType == Order_Type.CustomerService ? true : false;

    [NotMapped]
    public bool IsPaymentRequired => PaymentAgreement > Payment_Status.Unpaid ? true : false;

    [NotMapped]
    public bool IsPaymentPartial => PaymentAgreement == Payment_Status.PartiallyPaid ? true : false;

    [NotMapped]
    public double PriceTotalBYN { get => Currency == "BYN" ? PriceTotal : (PriceTotal * UnitToBYNConversion); }
}