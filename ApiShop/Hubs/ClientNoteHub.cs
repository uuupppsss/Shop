using ApiShop.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShopLib;

namespace ApiShop.Hubs
{
    public class ClientNoteHub:Hub
    {
        private readonly ShopdbContext _context;
        public ClientNoteHub(ShopdbContext context)
        {
            _context = context;
        }

        public async Task ProductUpdated(int product_id)
        {
            await Clients.All.SendAsync("ProductUpdated", product_id);
        }

        public async Task ProductSizesUpdated(int product_id)
        {
            await Clients.All.SendAsync("ProductSizesUpdated", product_id);
        }

        public async Task ProductImagesUpdated(int product_id)
        {
            await Clients.All.SendAsync("ProductImagesUpdated", product_id);
        }

        public async Task OrderUpdated(int order_id)
        {
            var order = await _context.Orders.Include(o => o.Status).FirstOrDefaultAsync(o=>o.Id==order_id);
            if (order == null) return;
            OrderDTO result = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                StatusId = order.StatusId,
                CreateDate = order.CreateDate,
                Adress = order.Adress,
                Index = order.Index,
                ContactPhone = order.ContactPhone,
                FullName = order.FullName,
                Trak = order.Trak,
                Cost = order.Cost,
                Status = order.Status.Title,
            };

            await Clients.All.SendAsync($"OrderUpdated", result);
        }

        public async Task BrandsUpdated()
        {
            await Clients.All.SendAsync($"BrandsUpdated");
        }

        public async Task TypesUpdated()
        {
            await Clients.All.SendAsync($"TypesUpdated");
        }

        public async Task ProductDeleted(int product_id)
        {
            await Clients.All.SendAsync("ProductDeleted", product_id);
        }

    }
}
