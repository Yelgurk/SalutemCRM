﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public enum Payment_Status {
    Unpaid,
    Reserved,
    PartiallyPaid,
    HalfPaid,
    AlmostPaid,
    FullyPaid
};

public enum Delivery_Status {
    InStock,
    NotShipped,
    PartiallyShippped,
    PartiallyDelivered,
    FullyShipped,
    FullyDelivered
};

public enum Task_Status {
    NotAvailable,
    NotStarted,
    Execution,
    Finished,
    Reassigned,
    Cancelled
};

public enum Order_Type {
    Manufacture,
    StockItem,
    Manufacture_StockItem
}; 