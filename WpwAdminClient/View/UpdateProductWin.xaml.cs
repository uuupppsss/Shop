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
using System.Windows.Shapes;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для UpdateProductWin.xaml
    /// </summary>
    public partial class UpdateProductWin : Window
    {
        public UpdateProductWin(int product_id)
        {
            InitializeComponent();
            DataContext = new UpdateProductWinVM(product_id);
        }
    }
}
