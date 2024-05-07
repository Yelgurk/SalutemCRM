using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain.Model;

public partial class Payment
{
    [NotMapped]
    public double PaymentValueBYN => Currency == "BYN" ? PaymentValue : (PaymentValue * UnitToBYNConversion);

    [NotMapped]
    public string RecordDate => RecordDT.ToShortDateString();
}