using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для OrdersListControl.xaml
    /// </summary>
    public partial class OrdersListControl : UserControl
    {
        public OrdersListControl()
        {
            InitializeComponent();
            DataContext = new OrdersListControlVM();
        }
    }
}
