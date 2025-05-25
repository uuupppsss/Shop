using ShopLib;
using System.Net.Http.Json;
using System.Windows;
using WpfAdminClient.Model;

namespace WpfAdminClient.Services
{
    public class AuthService
    {
        private static AuthService instance;
        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                    instance = new AuthService();
                return instance;
            }
        }

        private UserDTO _currentUser;
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnCurrentUserChanged();
            }
        }

        public event Action CurrentUserChanged;

        private void OnCurrentUserChanged()
        {
            CurrentUserChanged?.Invoke();
        }

        public bool IsAdmin()
        {
            return CurrentUser?.RoleId == 1|| CurrentUser?.RoleId == 3;
        }

        //public async Task SignUp(UserDTO user)
        //{
        //    try
        //    {
        //        string json=JsonSerializer.Serialize(user);
        //        await Client.HttpClient.PostAsync($"Auth/SignUp",new StringContent(json, Encoding.UTF8, "application/json"));
        //        MessageBox.Show("Регистрация прошла успешно! Выполните авторизацию");
        //    }
        //    catch(Exception ex) 
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        public async Task SignIn(string username,string password)
        {
            try
            {
                AuthResponse response = await Client.HttpClient
                    .GetFromJsonAsync<AuthResponse>($"Auth/SignIn/{username}/{password}");
                CurrentUser = response.User;
                Client.SetToken(response.Token);
                if (CurrentUser?.RoleId == 1)
                {
                    MessageBox.Show($"Авторизация прошла успешно! Добро пожаловать, {CurrentUser.Username}");
                }
                else
                {
                    MessageBox.Show("Что то пошло не так, пользователь не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SignOut()
        {
            CurrentUser = null; 
            Client.ResetHeaders();
        }
    }
}
