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
using WpfClientShop.ViewModel;

namespace WpfClientShop.View
{
    /// <summary>
    /// Логика взаимодействия для SignInControl.xaml
    /// </summary>
    public partial class SignInControl : UserControl
    {
        public SignInControl()
        {
            InitializeComponent();
            DataContext = new SignInControlVM();
            pwd_box.PasswordChar = '*';
            pwd_box.MaxLength = 50;
            ((SignInControlVM)DataContext).SetPassBox(pwd_box);
        }
    }
}
