﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SalutemCRM.Domain.Model;

public partial class Vendor
{
    [NotMapped]
    public Order? LastOrder => this.Orders.OrderBy(o => o.RecordDT).LastOrDefault();

    [NotMapped]
    public string LastOrderDT => this.LastOrder?.RecordDT.ToString("dd.MM.yyyy HH:mm:ss") ?? "{ нет }";
}