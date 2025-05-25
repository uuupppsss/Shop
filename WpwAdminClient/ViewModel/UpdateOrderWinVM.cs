using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAdminClient.Model;
using WpfAdminClient.Services;

namespace WpfAdminClient.ViewModel
{
    public class UpdateOrderWinVM:BaseVM
    {
        public CustomCommand SaveCommand { get; }

        private List<OrderStatusDTO> _statuses;
        public List<OrderStatusDTO> Statuses
        {
            get { return _statuses; }
            set 
            {
                _statuses = value;
                Signal();
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
            }
        }

        private string _trak;
        public string Trak
        {
            get { return _trak; }
            set 
            {
                _trak = value;
                Signal();
            }
        }


        private int _orderId;
        public UpdateOrderWinVM(int order_id)
        {
            _orderId = order_id;
            SaveCommand =new CustomCommand(Save);
            LoadData();
        }

        private async void LoadData()
        {
            Statuses = await AdminService.Instance.GetOrderStatuses();
        }

        private async void Save()
        {
            if(SelectedStatus==null)
            {
                MessageBox.Show("Выберите статус");
                return;
            }
            await AdminService.Instance.UpdateOrder(_orderId,SelectedStatus.Id,Trak);
        }
    }
}
