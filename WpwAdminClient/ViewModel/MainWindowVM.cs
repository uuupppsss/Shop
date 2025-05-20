using System.Windows;
using WpfAdminClient.Model;
using WpfAdminClient.Services;
using WpfAdminClient.View;


namespace WpfAdminClient.ViewModel
{
    public class MainWindowVM:BaseVM
    {
        public CustomCommand HomeCommand { get; }
        public CustomCommand ProductControlCommand { get; }
        public CustomCommand OrdersControlCommand { get; }
        public CustomCommand SignOutCommand { get; }

        public MainWindowVM()
        {
            HomeCommand = new CustomCommand(NavHome);
            ProductControlCommand = new CustomCommand(NavProductsControl);
            OrdersControlCommand = new CustomCommand(NavOrdersControl);
            SignOutCommand = new CustomCommand(SignOut);
        }

        private void NavHome()
        {
            if (IsAdmin())
            {
                var mainControl = new MainControl();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContentControl.Content = mainControl;
            }
            else
            {
                MessageBox.Show("Войдите в систему");
                NavSignIn();
            }

        }

        private void NavProductsControl()
        {

            if (IsAdmin())
            {
                var productsControl = new AddProductControl();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContentControl.Content = productsControl;
            }
            else
            {
                MessageBox.Show("Войдите в систему");
                NavSignIn();
            }
        }

        private void NavOrdersControl()
        {
            if(IsAdmin())
            {
                
            }
        }

        private bool IsAdmin()
        {
            return AuthService.Instance.CurrentUser?.RoleId == 1 ||
                 AuthService.Instance.CurrentUser?.RoleId == 3;
        }

        private void NavSignIn()
        {
            var authControl = new SignInControl();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = authControl;
        }

        private void SignOut()
        {

        }
    }
}
