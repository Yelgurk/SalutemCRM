using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public enum Payment_Status {
    Unpaid,
    PartiallyPaid,
    FullyPaid
};

public enum Delivery_Status {
    NotDelivered,
    PartiallyDelivered,
    FullyDelivered
};

public enum Task_Status {
    NotAvailable,
    AwaitPayment,
    AwaitStart,
    Execution,
    Finished,
    Reassigned,
    Cancelled
};

public enum Order_Type {
    ManagerSale,
    CustomerService,
    WarehouseRestocking
};

public enum Stock_Status {
    Enough,
    CloseToLimit,
    NotEnough,
    ZeroWarning
};

public enum User_Permission {
    None,
    Boss,
    Bookkeeper,
    SeniorSalesManager,
    SalesManager,
    ManufactureManager,
    ConstrEngineer,
    ManufactureEmployee,
    Storekeeper,
    PurchasingDepartment,
    ServiceDepartment
};

public static class EnumToString
{
    public static ObservableCollection<string> PaymentStatusToString { get; } = new() {
        "Нет оплаты",
        "Частичная оплата",
        "Полная оплата"
    };

    public static ObservableCollection<string> DeliverytStatusToString { get; } = new() {
        "Не отгружено",
        "Частичнно отгружено",
        "Полностью отгружено"
    };

    public static ObservableCollection<string> TaskStatusToString { get; } = new() {
        "Недоступно",
        "Ожидает оплаты",
        "Ожидает начала",
        "В процессе",
        "Переназначено",
        "Завершено",
        "Отменено"
    };

    public static ObservableCollection<string> OrderTypeToString { get; } = new() {
        "Продажа продукции",
        "Сервисное обслуживание",
        "Закупка на фирму"
    };

    public static ObservableCollection<string> StockStatusToString { get; } = new() {
        "1. достаток",
        "2. около лимита",
        "3. мало",
        "4. нет или < 1.0"
    };
}