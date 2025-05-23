using ShopLib;
using WpfClientShop.Services;

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

		
        public UserDataControlVM()
        {
            User=AuthService.Instance.CurrentUser;
        }

    }
}
