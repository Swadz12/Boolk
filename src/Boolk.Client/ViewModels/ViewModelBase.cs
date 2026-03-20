using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Boolk.Client.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action? OnStateChanged;

    protected void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        NotifyStateChanged();
        return true;
    }
}
