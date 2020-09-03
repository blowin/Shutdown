using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Shutdown.GUI.Annotations;

namespace Shutdown.GUI.Data
{
    [DataContract]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}