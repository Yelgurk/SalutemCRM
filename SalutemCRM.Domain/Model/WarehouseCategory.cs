using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public class WarehouseCategory : ModelBase
{
    [NotMapped]
    private int _id;
    [NotMapped]
    private string _name = null!;
    [NotMapped]
    private int _deep;
    [NotMapped]
    private int? _parentCategoryForeignKey;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get => _id; set => OnPropertyChanged(ref _id, value); }

    [MaxLength(200)]
    public string Name { get => _name; set => OnPropertyChanged(ref _name, value); }

    public int Deep { get => _deep; set => OnPropertyChanged(ref _deep, value); }

    public int? ParentCategoryForeignKey { get => _parentCategoryForeignKey; set => OnPropertyChanged(ref _parentCategoryForeignKey, value); }
    [ForeignKey("ParentCategoryForeignKey")]
    public WarehouseCategory? ParentCategory { get; set; }

    public List<WarehouseCategory> SubCategories { get; set; } = new();
    public List<WarehouseItem> WarehouseItems { get; set; } = new();
}
