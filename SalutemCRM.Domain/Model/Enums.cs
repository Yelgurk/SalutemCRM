﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public enum Payment_Status {
    Unpaid,
    PartiallyPaid,
    HalfPaid,
    AlmostPaid,
    FullyPaid
};

public enum Delivery_Status {
    NotShipped,
    PartiallyShippped,
    PartiallyDelivered,
    FullyShipped,
    FullyDelivered
};

public enum Task_Status {
    NotStarted,
    Execution,
    Finished,
    Reassigned
};