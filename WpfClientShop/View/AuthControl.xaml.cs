using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClientShop.View
{
    /// <summary>
    /// Логика взаимодействия для AuthControl.xaml
    /// </summary>
    public partial class AuthControl : UserControl
    {
        public AuthControl()
        {
            InitializeComponent();
            MainContentControl.Content = new SignInControl();
        }

        private void SignInClick(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = new SignInControl();
        }

        private void SignUpClick(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = new SignUpControl();
        }
    }
}
