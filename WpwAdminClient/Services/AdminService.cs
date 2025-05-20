using Microsoft.AspNetCore.SignalR.Client;
using ShopLib;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using WpfAdminClient.Model;

namespace WpfAdminClient.Services
{
    public class AdminService
    { 
        HubConnection _connection;
        private static AdminService instance;
        public static AdminService Instance
        {
            get
            {
                if (instance == null)
                    instance = new AdminService();
                return instance;
            }
        }
        public AdminService()
        {
            InitializeAdminConnection();
        }
        public event Action OrdersCollectionChanged;

        public async void InitializeAdminConnection()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5226/adminshub")
                .Build();
            await _connection.StartAsync();
        }

        public async Task AddNewProduct(ProductDTO product, List<byte[]> images)
        {
            try
            {
                string json=JsonSerializer.Serialize(product);
                var responce = await Client.HttpClient.PostAsync("Products",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                int addedProductId = await responce.Content.ReadFromJsonAsync<int>();
                await AddProductImages(addedProductId, images);
                await _connection.InvokeAsync("ProductsCollectionChanged");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task AddProductImages(int product_id,List<byte[]> images)
        {
            try
            {
                string json = JsonSerializer.Serialize(images);
                await Client.HttpClient.PostAsync($"Productimages/{product_id}", 
                    new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task AddNewCategory(string title)
        {
            try
            {
                await Client.HttpClient.GetAsync($"Producttypes/{title}");
                await _connection.InvokeAsync("TypesCollectionChanged");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Категория успешно добавлена");
        }

        public async Task AddNewBrand(string title)
        {
            try
            {
                await Client.HttpClient.GetAsync($"Brands/{title}");
                await _connection.InvokeAsync("BrandsCollectionChanged");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Брэнд успешно добавлен");
        }

        public async Task RemoveCategory(int category_id)
        {
            try
            {
                await Client.HttpClient.DeleteAsync($"Producttypes/{category_id}");
                await _connection.InvokeAsync("TypesCollectionChanged");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Категория удалена успешно");
        }

        public async Task RemoveBrand(int brand_id)
        {
            try
            {
                await Client.HttpClient.DeleteAsync($"Brands/{brand_id}");
                await _connection.InvokeAsync("BrandsCollectionChanged");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Бренд удален успешно");
        }

        public async Task<List<OrderDTO>> GetOrdersList(int status_id=0)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<OrderDTO>>($"Orders/List/{status_id}");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<OrderDTO> GetOrderDetails(int order_id)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<OrderDTO>($"Orders/{order_id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<List<OrderStatusDTO>> GetOrderStatuses()
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<OrderStatusDTO>>("Orderstatus");
            }
            catch( Exception ex )
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

    }
}
