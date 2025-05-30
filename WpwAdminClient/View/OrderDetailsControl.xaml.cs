﻿using System;
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
using WpfAdminClient.ViewModel;

namespace WpfAdminClient.View
{
    /// <summary>
    /// Логика взаимодействия для OrderDetailsControl.xaml
    /// </summary>
    public partial class OrderDetailsControl : UserControl
    {
        public OrderDetailsControl(int order_id)
        {
            InitializeComponent();
            DataContext = new OrderDetailsControlVM(order_id);
        }
    }
}
