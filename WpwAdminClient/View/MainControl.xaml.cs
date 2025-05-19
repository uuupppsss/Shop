using System.Windows;
using System.Windows.Controls;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
            DataContext = new MainControlVM();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
