using System.Windows;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();
            MainContentControl.Content=new SignInControl();
        }
    }
}