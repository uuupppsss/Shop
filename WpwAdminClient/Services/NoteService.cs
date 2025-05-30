﻿using Microsoft.AspNetCore.SignalR.Client;
using ShopLib;
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

        private HubConnection _clientconnection;
        private HubConnection _adminconnection;
        public NoteService()
        {
            InitializeGetingNotes();
        }

        public event Action<int> ProductUpdated;
        public event Action OrderCreated;
        public event Action BrandsUpdated;
        public event Action TypesUpdated;
        public event Action<OrderDTO> OrderUpdated;
        public event Action<int> ProductDeleted;
        public event Action<int> ProductImagesUpdated;
        public event Action<int> ProductSizesUpdated;

        private async void InitializeGetingNotes()
        {
            _clientconnection = new HubConnectionBuilder()
                           .WithUrl("http://localhost:5226/clientshub").Build();

            _clientconnection.On<int>("ProductUpdated", (product_id) =>
            {
                ProductUpdated?.Invoke(product_id);
            });

            _clientconnection.On<int>("ProductImagesUpdated", (product_id) =>
            {
                ProductImagesUpdated?.Invoke(product_id);
            });

            _clientconnection.On<int>("ProductSizesUpdated", (product_id) =>
            {
                ProductSizesUpdated?.Invoke(product_id);
            });

            _clientconnection.On<int>("ProductDeleted", (product_id) =>
            {
                ProductDeleted?.Invoke(product_id);
            });

            _clientconnection.On("BrandsUpdated", () =>
            {
                BrandsUpdated?.Invoke();
            });

            _clientconnection.On("TypesUpdated", () =>
            {
                TypesUpdated?.Invoke();
            });

            _clientconnection.On<OrderDTO>("OrderUpdated", (order) =>
            {
                OrderUpdated?.Invoke(order);
            });

            await _clientconnection.StartAsync();

            _adminconnection = new HubConnectionBuilder()
                          .WithUrl("http://localhost:5226/adminshub").Build();

            _adminconnection.On("OrderCreated", () =>
            {
                OrderCreated?.Invoke();
                MessageBox.Show("Создан новый заказ");
            });

            await _adminconnection.StartAsync();
        }
    }
}
