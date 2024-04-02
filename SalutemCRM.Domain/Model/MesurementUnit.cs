using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain.Model;

public partial class MesurementUnit : ObservableObject
{
    [NotMapped]
    [ObservableProperty]
    private string _name = null!;

    public object Clone() { return this.MemberwiseClone(); }
}