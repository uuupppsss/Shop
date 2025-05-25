using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientShop.Services;

namespace WpfClientShop.ViewModel
{
    public class OrdersControlVM:BaseVM
    {
        private List<OrderDTO> _orders;
        public List<OrderDTO> Orders
        {
            get { return _orders; }
            set 
            {
                _orders = value;
                Signal();
            }
        }

        public OrdersControlVM()
        {
            LoadData();
            NoteService.Instance.OrderUpdated += LoadData;
        }

        private async void LoadData()
        {
            Orders=await ClientService.Instance.GetUserOrders();
        }
    }
}
