using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientShop.Model;

namespace WpfClientShop.Convert
{
    public class ProductConverter
    {
        private static ProductConverter instance;
        public static ProductConverter Instance
        {
            get
            {
                if (instance == null)
                    instance = new ProductConverter();
                return instance;
            }
        }

        public List<ProductDisplay> ConvertToProductDisplay(List<ProductDTO> products)
        {
           if(products!=null)
            {
                return products.Select(product => new ProductDisplay
                {
                    Id = product.Id,
                    Title = product.Title,
                    Description = product.Description,
                    Price = product.Price,
                    TypeId = product.TypeId,
                    BrandId = product.BrandId,
                    TimeBought = product.TimeBought,
                    HeaderImage = product.HeaderImage
                }).ToList();
            }
            return null;
        }
    }
}
