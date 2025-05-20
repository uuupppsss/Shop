using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAdminClient.Services;

namespace WpfAdminClient.ViewModel
{
    public class OrderDetailsControlVM:BaseVM
    {
        private int _orderId;

        private OrderDTO _order;
        public OrderDTO Order
        {
            get { return _order; }
            set 
            {
                _order = value;
                Signal();
            }
        }


        public OrderDetailsControlVM(int order_id)
        {
            _orderId=order_id;
        }
        public OrderDetailsControlVM()
        {
            LoadData();
        }

        private async void LoadData()
        {
            Order = await AdminService.Instance.GetOrderDetails(_orderId);
        }
    }
}
