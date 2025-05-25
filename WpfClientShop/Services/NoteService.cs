using Microsoft.AspNetCore.SignalR.Client;
using ShopLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClientShop.Services
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

        private HubConnection connection;
        public NoteService()
        {
            InitializeGetingNotes();
        }

        public event Action<int> ProductUpdated;
        public event Action OrderUpdated;
        public event Action BrandsUpdated;
        public event Action TypesUpdated;
        public event Action<int> ProductDeleted;

        public event Action<int> ProductImagesUpdated;
        public event Action<int> ProductSizesUpdated;

        private async void InitializeGetingNotes()
        {
            connection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();

            connection.On<int>("ProductDeleted", (product_id) =>
            {
                ProductDeleted?.Invoke(product_id);
            });

            connection.On<int>("ProductUpdated", (product_id) =>
            {
                ProductUpdated?.Invoke(product_id);
            });

            connection.On<int>("ProductImagesUpdated", (product_id) =>
            {
                ProductImagesUpdated?.Invoke(product_id);
            });

            connection.On<int>("ProductSizesUpdated", (product_id) =>
            {
                ProductSizesUpdated?.Invoke(product_id);
            });

            connection.On<OrderDTO>("OrderUpdated", (order) =>
            {
                
                if(order.UserId==AuthService.Instance.CurrentUser?.Id)
                {
                    OrderUpdated?.Invoke();
                    MessageBox.Show($"Заказ #{order.Id} обновлен");
                }
                
            });

            connection.On("BrandsUpdated", () =>
            {
                BrandsUpdated?.Invoke();
            });
            connection.On("TypesUpdated", () =>
            {
                TypesUpdated?.Invoke();
            });

            await connection.StartAsync();
        }
    }
}
