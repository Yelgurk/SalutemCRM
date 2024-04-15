using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class MaterialFlow : ClonableObservableObject<MaterialFlow>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int? _returnedForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _employeeForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int _warehouseSupplyForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _manufactureForeignKey;

    [NotMapped]
    [ObservableProperty]
    private int? _orderForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string _additionalInfo = null!;

    [NotMapped]
    [ObservableProperty]
    private double _countReservedFromStock;

    [NotMapped]
    [ObservableProperty]
    private double _countUsed;

    [NotMapped]
    [ObservableProperty]
    private double _countReturnedToStock;

    [NotMapped]
    [ObservableProperty]
    private Delivery_Status _deliveryStatus;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;



    [NotMapped]
    [ObservableProperty]
    private MaterialFlow? _returnedMaterial;

    [NotMapped]
    [ObservableProperty]
    private User? _employee;

    [NotMapped]
    [ObservableProperty]
    private WarehouseSupply? _warehouseSupply;

    [NotMapped]
    [ObservableProperty]
    private Manufacture? _manufacture;

    [NotMapped]
    [ObservableProperty]
    private Order? _order;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<MaterialFlow> _exchangedMaterials = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<FileAttach> _fileAttachs = new();
}
