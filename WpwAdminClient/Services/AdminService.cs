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

        public async Task AddNewProduct(ProductDTO product, List<byte[]> images)
        {
            try
            {
                string json=JsonSerializer.Serialize(product);
                var responce = await Client.HttpClient.PostAsync("Products",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                int addedProductId = await responce.Content.ReadFromJsonAsync<int>();
                await AddProductImages(addedProductId, images);
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

            }
            catch
            {

            }
        }
    }
}
