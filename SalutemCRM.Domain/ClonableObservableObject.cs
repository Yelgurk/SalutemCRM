using CommunityToolkit.Mvvm.ComponentModel;

namespace SalutemCRM.Domain;

public abstract class ClonableObservableObject<T> : ObservableObject
{
    public T Clone() => (T)this.MemberwiseClone();
}
