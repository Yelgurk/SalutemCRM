
using ReactiveUI;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class Client : ModelBase
{
    [NotMapped]
    private int _id;
    [NotMapped]
    private string _name = null!;
    [NotMapped]
    private string _address = null!;
    [NotMapped]
    private string? _additionalInfo;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get => _id; set => OnPropertyChanged(ref _id, value); }

    [StringLength(200)]
    public string Name { get => _name; set => OnPropertyChanged(ref _name, value); }

    [StringLength(200)]
    public string Address { get => _address; set => OnPropertyChanged(ref _address, value); }

    [StringLength(200)]
    public string? AdditionalInfo { get => _additionalInfo; set => OnPropertyChanged(ref _additionalInfo, value); }

    public List<OfficeOrder> OfficeOrders { get; set; } = new();
}