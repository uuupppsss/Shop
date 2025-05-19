using System.Windows.Controls;
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для ProductDetailsControl.xaml
    /// </summary>
    public partial class ProductDetailsControl : UserControl
    {
        public ProductDetailsControl(int product_id)
        {
            InitializeComponent();
            DataContext = new ProductDetailsVM(product_id);
        }
    }
}
