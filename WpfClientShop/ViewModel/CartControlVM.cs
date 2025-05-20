using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientShop.ViewModel
{
    public class CartControlVM:BaseVM
    {
        private List<BasketItemDTO> _basket;

        public List<BasketItemDTO> Basket
        {
            get { return _basket; }
            set { _basket = value; }
        }

        public CartControlVM()
        {
            
        }
    }
}
