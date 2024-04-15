
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class Logging : ClonableObservableObject<Logging>
{
    [NotMapped]
    [ObservableProperty]
    private int _id;

    [NotMapped]
    [ObservableProperty]
    private int _userId;

    [NotMapped]
    [ObservableProperty]
    private string _userLogin = null!;

    [NotMapped]
    [ObservableProperty]
    private string _userFirstName = null!;

    [NotMapped]
    [ObservableProperty]
    private string _userLastName = null!;

    [NotMapped]
    [ObservableProperty]
    private string _sQLQuery = null!;

    [NotMapped]
    [ObservableProperty]
    private DateTime _recordDT;
}
