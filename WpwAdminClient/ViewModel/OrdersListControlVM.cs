using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAdminClient.Model;
using WpfAdminClient.Services;
using WpfAdminClient.View;

namespace WpfAdminClient.ViewModel
{
    public class OrdersListControlVM:BaseVM
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

        private List<OrderStatusDTO> _statuses;
        public List<OrderStatusDTO> Statuses
        {
            get { return _statuses; }
            set 
            {
                _statuses = value;
                Signal();
                UpdateData();
            }
        }

        private OrderStatusDTO _selectedStatus;
        public OrderStatusDTO SelectedStatus
        {
            get { return _selectedStatus; }
            set 
            {
                _selectedStatus = value;
                Signal();
                UpdateData();
            }
        }


        public CustomCommand<int> ViewDetailsCommand { get; set; }

        public OrdersListControlVM()
        {
            ViewDetailsCommand = new CustomCommand<int>(ViewDetails);

            LoadData();
            
        }

        private async void LoadData()
        {
            Statuses = await AdminService.Instance.GetOrderStatuses();
            Statuses.Insert(0, new OrderStatusDTO { Id = 0, Title = "Все" });
            SelectedStatus = Statuses[0];
            NoteService.Instance.OrdersCollectionChanged += UpdateData;
            Orders = await AdminService.Instance.GetOrdersList(SelectedStatus.Id);
        }

        private async void UpdateData()
        {
            if (SelectedStatus != null)
            {
                Orders = await AdminService.Instance.GetOrdersList(SelectedStatus.Id);
            }
        }

        private void ViewDetails(int order_id)
        {
            var ordersControl = new OrderDetailsControl(order_id);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainContentControl.Content = ordersControl;
        }

    }
}
