using System.Windows.Controls;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для AddProductControl.xaml
    /// </summary>
    public partial class AddProductControl : UserControl
    {
        public AddProductControl()
        {
            InitializeComponent();
            DataContext = new AddProductVM();
        }

       
    }
}
