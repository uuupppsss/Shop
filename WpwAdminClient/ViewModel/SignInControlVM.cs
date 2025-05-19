using System.Windows.Controls;
using WpfAdminClient.Model;
using WpfAdminClient.Services;

namespace WpfAdminClient.ViewModel
{
    public class SignInControlVM:BaseVM
    {
		private string _username;
		public string Username
		{
			get { return _username; }
			set 
			{
				_username = value;
				Signal();
			}
		}

		private PasswordBox _pwdBox;

		public CustomCommand SignInCommand { get; }

        internal void SetPassBox(PasswordBox pwd_box)
        {
            _pwdBox = pwd_box;
        }
        public SignInControlVM()
        {
			SignInCommand = new CustomCommand(SignIn);
        }

		private async void SignIn()
		{
			await AuthService.Instance.SignIn(Username, _pwdBox.Password);
		}
    }
}
