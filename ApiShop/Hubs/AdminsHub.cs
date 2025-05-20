using ApiShop.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShopLib;

namespace ApiShop.Hubs
{
    public class AdminsHub:Hub
    {
        private ShopdbContext _context;
        public AdminsHub(ShopdbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="admin")]
        public async Task TypesCollectionChanged()
        {
            await Clients.All.SendAsync("TypesCollectionChanged");
        }

        public async Task BrandsCollectionChanged()
        {
            await Clients.All.SendAsync("BrandsCollectionChanged");
        }

        public async Task ProductsCollectionChanged()
        {
            await Clients.All.SendAsync("ProductsCollectionChanged");
        }

        public async Task OrderUpdated(int order_id)
        {
            Order order=await _context.Orders.FirstOrDefaultAsync(o=>o.Id==order_id);
            if (order!=null)
            {
                await Clients.Groups($"{order.UserId}").SendAsync("OrderUpdated",order_id);
            }
        }
    }
}
