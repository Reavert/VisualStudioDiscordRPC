using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
                
            storage = value;
            ForceUpdate(propertyName);
            
            return true;
        }

        protected virtual void ForceUpdate([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
