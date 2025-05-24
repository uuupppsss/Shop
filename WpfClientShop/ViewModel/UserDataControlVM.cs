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
        public UserDataControlVM()
        {
            User=AuthService.Instance.CurrentUser;
			ViewOrdersCommand = new CustomCommand(ViewOrders);
        }

		private void ViewOrders()
		{
            var ordersControl = new OrdersControl();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = ordersControl;
        }
    }
}
