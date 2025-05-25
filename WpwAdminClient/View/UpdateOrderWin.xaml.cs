using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для UpdateOrderWin.xaml
    /// </summary>
    public partial class UpdateOrderWin : Window
    {
        public UpdateOrderWin(int order_id)
        {
            InitializeComponent();
            DataContext = new UpdateOrderWinVM(order_id);
        }
    }
}
