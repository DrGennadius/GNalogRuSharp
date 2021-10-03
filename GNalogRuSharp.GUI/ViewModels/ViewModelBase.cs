using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GNalogRuSharp.GUI.ViewModels
{
    /// <summary>
    /// Базовый класс вьюмодельки.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
