﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class FileAttach : ClonableObservableObject<FileAttach>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _warehouseItemForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _materialFlowForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _paymentForeignKey;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;

    [NotMapped]
    [ObservableProperty]
    private string _fileName = null!;



    [NotMapped]
    [ObservableProperty]
    private Order? _order;

    [NotMapped]
    [ObservableProperty]
    private WarehouseItem? _warehouseItem;

    [NotMapped]
    [ObservableProperty]
    private MaterialFlow? _materialFlow;

    [NotMapped]
    [ObservableProperty]
    private Payment? _payment;
}