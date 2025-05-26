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

        public async Task AddNewProduct(ProductDTO product, List<byte[]> images,List<ProductSizeDTO> sizes)
        {
            try
            {
                string json=JsonSerializer.Serialize(product);
                var responce = await Client.HttpClient.PostAsync("Products",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                int addedProductId = await responce.Content.ReadFromJsonAsync<int>();
                await AddProductImages(addedProductId, images);
                await AddProductSizes(addedProductId, sizes);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Данные успешно сохранены");
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

        private async Task AddProductSizes(int product_id, List<ProductSizeDTO> sizes)
        {
            try
            {
                string json = JsonSerializer.Serialize(sizes);
                await Client.HttpClient.PostAsync($"Productsizes/{product_id}",
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
                var result=await Client.HttpClient.GetAsync($"Brands/{title}");
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
                var responce=await Client.HttpClient.DeleteAsync($"Producttypes/{category_id}");
                if (!responce.IsSuccessStatusCode)
                {
                    MessageBox.Show(await responce.Content.ReadAsStringAsync());
                    return;
                }
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
                var responce=await Client.HttpClient.DeleteAsync($"Brands/{brand_id}");
                if (!responce.IsSuccessStatusCode)
                {
                    MessageBox.Show(await responce.Content.ReadAsStringAsync());
                    return;
                }
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

        public async Task UpdateProduct(ProductDTO product, List<ProductSizeDTO> sizes, List<byte[]> images)
        {
            try
            {
                string json1=JsonSerializer.Serialize(product);
                await Client.HttpClient.PutAsync("Products", 
                    new StringContent(json1, Encoding.UTF8, "application/json"));
                string json2 = JsonSerializer.Serialize(sizes);
                var responce = await Client.HttpClient.PutAsync($"Productsizes/{product.Id}",
                    new StringContent(json2, Encoding.UTF8, "application/json"));
                if(images?.Count>0)
                {
                    string json3 = JsonSerializer.Serialize(images);
                    var responce3 = await Client.HttpClient.PutAsync($"Productimages/{product.Id}",
                        new StringContent(json3, Encoding.UTF8, "application/json"));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show("Данные успешно сохранены");
            });
        }
    
        public async Task UpdateOrder(int id, int status_id, string trak)
        {
            if (trak == null) trak = "-";
            try
            {
                await Client.HttpClient.GetAsync($"Orders/Update/{id}/{status_id}/{trak}");
            }
            catch ( Exception ex )
            {
                MessageBox.Show (ex.Message);
                return;
            }
            MessageBox.Show("Данные успешно обновлены");
        }

        public async Task<List<OrderItemDTO>> GetOrderItems(int id)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<OrderItemDTO>>($"Orders/Items/{id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
                return null;
            }
        }

        public async Task RemoveProduct(int id)
        {
            try
            {
                var responce=await Client.HttpClient.DeleteAsync($"Products/{id}");
                if(!responce.IsSuccessStatusCode)
                {
                    MessageBox.Show(await responce.Content.ReadAsStringAsync());
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Продукт удален успешно");
        }
    }
}
