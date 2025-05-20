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

        public async Task<List<ProductDTO>> GetProductsList(string? filterword = null, int typde_id = 0, int brand_id = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(filterword)) filterword = "-";
                return await Client.HttpClient.GetFromJsonAsync<List<ProductDTO>>($"Products/{filterword}/{typde_id}/{brand_id}");
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

        public async Task<List<ProductImageDTO>> GetProductImages(int product_id)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<ProductImageDTO>>($"Productimages/{product_id}");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<ProductDTO> GetProductDetails(int product_id)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<ProductDTO>($"Products/{product_id}");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        public async Task<List<ProductSizeDTO>> GetProductSizes(int product_id)
        {
            try
            {
                return await Client.HttpClient.GetFromJsonAsync<List<ProductSizeDTO>>($"Productsizes/{product_id}");
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
