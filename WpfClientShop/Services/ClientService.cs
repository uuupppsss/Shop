using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfClientShop.Model;

namespace WpfClientShop.Services
{
    public class ClientService
    {
        private static ClientService instance;
        public static ClientService Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientService();
                return instance;
            }
        }

        public async Task AddProductToBasket(int product_id, string size)
        {
            try
            {
                var responce=await Client.HttpClient.GetAsync($"Basketitems/Create/{product_id}/{size}");
                if(responce.IsSuccessStatusCode) MessageBox.Show("Товар добавлен в корзину");
                else MessageBox.Show($"Ошибка: {responce.StatusCode}","Что то пошло не так");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            
        }
    }
}
