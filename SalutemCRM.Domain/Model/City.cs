using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class City : ClonableObservableObject<City>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _countryForeignKey;

    [NotMapped]
    [ObservableProperty]
    private string _name = null!;



    [NotMapped]
    [ObservableProperty]
    private Country? _country;



    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<RegionMonitoring> _regionsMonitoring = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Client> _clients = new();

    [NotMapped]
    [ObservableProperty]
    private ObservableCollection<Vendor> _vendors = new();
}