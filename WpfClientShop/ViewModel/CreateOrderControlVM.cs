using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfClientShop.Model;
using WpfClientShop.Services;
using WpfClientShop.View;

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
            Order.CreateDate = DateTime.Now;
            await ClientService.Instance.CreateOrder(Order);
            var orders = new OrdersControl();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = orders;
        }
    }
}
