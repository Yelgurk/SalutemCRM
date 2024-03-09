using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Domain;

[NotMapped]
public class ModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public TRet OnPropertyChanged<TRet>(
        ref TRet backingField,
        TRet newValue,
        [CallerMemberName] string? propertyName = null)
    {
        backingField = newValue;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? ""));
        return newValue;
    }
}
