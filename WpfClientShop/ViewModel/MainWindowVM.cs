using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfClientShop.Model;
using WpfClientShop.Services;
using WpfClientShop.View;

namespace WpfClientShop.ViewModel
{
    public class MainWindowVM:BaseVM
    {

        private bool _isAuthorized;
        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set
            {
                _isAuthorized = value;
                Signal();
            }
        }

        public CustomCommand HomeCommand { get; }

        public CustomCommand CartCommand { get; }

        public CustomCommand AccountCommand { get; }

        public MainWindowVM()
        {
            AuthService.Instance.CurrentUserChanged += OnCurrentUserChanged;
            UpdateUserPolicy();
            HomeCommand = new CustomCommand(NavHome);
            CartCommand = new CustomCommand(NavCart);
            AccountCommand=new CustomCommand(NavAccount);
        }

        

        private void OnCurrentUserChanged()
        {
            UpdateUserPolicy();
        }

        private void UpdateUserPolicy()
        {
            IsAuthorized=AuthService.Instance.IsAuthorized();
            Signal(nameof(IsAuthorized));
        }



        private void NavHome()
        {
            var mainControl = new MainControl();
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.DataContext == this);
            mainWindow.MainContentControl.Content = mainControl;
        }

        private void NavCart()
        {
            if(IsAuthorized)
            {
                var cartControl = new CartControl();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContentControl.Content = cartControl;
            }
            else
            {
                var authControl = new AuthControl();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContentControl.Content = authControl;
            }
        }

        private void NavAccount()
        {
            if(IsAuthorized)
            {
                var accountControl = new UserDataControl();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContentControl.Content = accountControl;
            }
            else
            {
                var authControl = new AuthControl();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.MainContentControl.Content = authControl;
            }
        }
    }
}
