using ShopLib;
using System.Windows;
using WpfClientShop.Model;
using WpfClientShop.Services;
using WpfClientShop.View;

namespace WpfClientShop.ViewModel
{
    public class UserDataControlVM:BaseVM
    {
		private UserDTO _user;
		public UserDTO User
		{
			get { return _user; }
			set 
			{
				_user = value;
				Signal();
			}
		}

		public CustomCommand ViewOrdersCommand { get; }
		public CustomCommand SignOutCommand { get; }

        public UserDataControlVM()
        {
            User=AuthService.Instance.CurrentUser;
			ViewOrdersCommand = new CustomCommand(ViewOrders);
			SignOutCommand=new CustomCommand(SignOut);
        }

		private void ViewOrders()
		{
            var ordersControl = new OrdersControl();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = ordersControl;
        }
		private void SignOut()
		{
			AuthService.Instance.SignOut();
            var home = new MainControl();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = home;
        }
    }
}
