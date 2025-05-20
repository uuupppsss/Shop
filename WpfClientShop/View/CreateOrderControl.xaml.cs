using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfClientShop.ViewModel;

namespace WpfClientShop.View
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderControl.xaml
    /// </summary>
    public partial class CreateOrderControl : UserControl
    {
        public CreateOrderControl(List<OrderItemDTO> items,decimal cost)
        {
            InitializeComponent();
            DataContext = new CreateOrderControlVM(items,cost);
        }
    }
}
