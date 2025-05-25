using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfClientShop.Model;

namespace WpfClientShop.Services
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

        public bool IsAuthorized()
        {
            return CurrentUser?.RoleId==2;
        }

        public async Task SignUp(UserDTO user)
        {
            try
            {
                string json=JsonSerializer.Serialize(user);
                await Client.HttpClient.PostAsync($"Auth/SignUp",new StringContent(json, Encoding.UTF8, "application/json"));
                
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Регистрация прошла успешно! Выполните авторизацию");

        }

        public async Task SignIn(string username,string password)
        {
            try
            {
                AuthResponse response = await Client.HttpClient
                    .GetFromJsonAsync<AuthResponse>($"Auth/SignIn/{username}/{password}");
                CurrentUser = response.User;
                Client.SetToken(response.Token);
                if (CurrentUser?.RoleId == 2)
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
