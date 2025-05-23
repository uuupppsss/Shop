using ShopLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<int> GetBasketItemMaxCount(int item_id)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<int>($"Basketitems/MaxCount/{item_id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public async Task<List<BasketItemDTO>> GetUserBasket()
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<BasketItemDTO>>($"Basketitems");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task UpdateBasketItem(int id, int count)
        {
            try
            {
                await Client.HttpClient.GetAsync($"Basketitems/Update/{id}/{count}");
            }
            catch( Exception ex ) 
            {
                MessageBox.Show(ex.Message );
            }
        }

        public async Task CreateOrder(OrderDTO order)
        {
            try
            {
                string json = JsonSerializer.Serialize(order);
                await Client.HttpClient.PostAsync("Orders", 
                    new StringContent(json,Encoding.UTF8, "application/json"));

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Зааз успешно добавлен");
        }

        public async Task<List<OrderDTO>> GetUserOrders()
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<OrderDTO>>("Orders/User");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
