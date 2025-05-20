using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAdminClient.Services
{
    public class NoteService
    {
        private static NoteService instance;
        public static NoteService Instance
        {
            get
            {
                if (instance == null)
                    instance = new NoteService();
                return instance;
            }
        }

        HubConnection _connection;

        public NoteService()
        {
            InitializeAdminConnection();
        }

        public event Action ProductsCollectionChanged;
        public event Action BrandsCollectionChanged;
        public event Action TypesCollectionChanged;
        public event Action OrdersCollectionChanged;

        private async void InitializeAdminConnection()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5226/adminshub")
                .Build();

            _connection.On("ProductsCollectionChanged", () =>
            {
                ProductsCollectionChanged?.Invoke();
            });
            _connection.On("BrandsCollectionChanged", () =>
            {
                BrandsCollectionChanged?.Invoke();
            });

            _connection.On("TypesCollectionChanged", () =>
            {
                TypesCollectionChanged?.Invoke();
            });

            _connection.On<int>("OrderCreated", order_id  =>
            {
                MessageBox.Show($"Новый заказ №{order_id}");
                OrdersCollectionChanged?.Invoke();
            });

            _connection.On<int>("OrderCanceled", order_id =>
            {
                MessageBox.Show($"Заказ №{order_id} отменен");
                OrdersCollectionChanged?.Invoke();
            });

            await _connection.StartAsync();
        }
    }
}
