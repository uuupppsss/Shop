using System.Windows.Controls;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
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
