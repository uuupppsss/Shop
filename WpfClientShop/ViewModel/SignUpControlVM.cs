using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfClientShop.Model;
using WpfClientShop.Services;

namespace WpfClientShop.ViewModel
{
    public class SignUpControlVM:BaseVM
    {
        private PasswordBox _pwdBox;
        private PasswordBox _repeatPwdBox;

        internal void SetPassBoxes(PasswordBox pwd_box,PasswordBox repeatpwd_box)
        {
            _pwdBox = pwd_box;
            _repeatPwdBox = repeatpwd_box;
        }

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

        public CustomCommand SignUpCommand { get; }

        public SignUpControlVM()
        {
            User=new UserDTO();
            SignUpCommand = new CustomCommand(SignUp);
        }

        private async void SignUp()
        {
            if(_pwdBox.Password!=_repeatPwdBox.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            User.Password = _pwdBox.Password;
            await AuthService.Instance.SignUp(User);
        }
    }
}
