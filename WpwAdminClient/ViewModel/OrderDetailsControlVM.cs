using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAdminClient.Model;
using WpfAdminClient.Services;
using WpfAdminClient.View;

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

        private List<OrderItemDTO> _items;
        public List<OrderItemDTO> Items
        {
            get { return _items; }
            set 
            {
                _items = value; 
                Signal() ;
            }
        }

        public CustomCommand EditCommand { get; }
        public OrderDetailsControlVM(int order_id)
        {
            _orderId=order_id;
            EditCommand = new CustomCommand(Edit);
            LoadData();
            NoteService.Instance.OrderUpdated += OrderUpdated;
        }


        private async void LoadData()
        {
            Order = await AdminService.Instance.GetOrderDetails(_orderId);
            Items = await AdminService.Instance.GetOrderItems(_orderId);
        }

        private void OrderUpdated(OrderDTO order)
        {
            if(_orderId==order.Id)
            {
                LoadData();
            }
        }

        private async void Edit()
        {
            var win = new UpdateOrderWin(_orderId);
            win.ShowDialog();
        }
    }
}
