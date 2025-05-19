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
    /// Логика взаимодействия для SignUpControl.xaml
    /// </summary>
    public partial class SignUpControl : UserControl
    {
        public SignUpControl()
        {
            InitializeComponent();
            DataContext = new SignUpControlVM();
            pwd_box.PasswordChar = '*';
            pwd_box.MaxLength = 50;
            repeat_pwd_box.PasswordChar = '*';
            repeat_pwd_box.MaxLength = 50;
            ((SignUpControlVM)DataContext).SetPassBoxes(pwd_box,repeat_pwd_box);
        }
    }
}
