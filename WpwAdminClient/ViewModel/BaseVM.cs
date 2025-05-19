using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfAdminClient.ViewModel
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Signal([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(prop));
        }
    }
}
