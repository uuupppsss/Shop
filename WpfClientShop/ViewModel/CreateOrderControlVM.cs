using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientShop.Model;
using WpfClientShop.Services;

namespace WpfClientShop.ViewModel
{
    public class CreateOrderControlVM:BaseVM
    {
        private List<OrderItemDTO> _items;
        public List<OrderItemDTO> Items
        {
            get { return _items; }
            set 
            {
                _items = value;
                Signal();
            }
        }

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


        public CustomCommand CreateOrder { get; }

        public CreateOrderControlVM(List<OrderItemDTO> items, decimal cost)
        {
            Order = new OrderDTO {Cost=cost };
            Items= items;
            CreateOrder = new CustomCommand(Create);
        }

        public async void Create()
        {
            await ClientService.Instance.CreateOrder(Order);
        }
    }
}
