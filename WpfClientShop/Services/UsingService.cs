using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfClientShop.Model;

namespace WpfClientShop.Services
{
    public class UsingService
    {
        private static UsingService instance;
        public static UsingService Instance
        {
            get
            {
                if (instance == null)
                    instance = new UsingService();
                return instance;
            }
        }

        public async Task<List<ProductDTO>> GetProductsList()
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<ProductDTO>>("Products");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<List<BrandDTO>> GetBrandsList()
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<BrandDTO>>("Brands");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<List<ProductTypeDTO>> GetTypesList()
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<ProductTypeDTO>>("Producttypes");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
