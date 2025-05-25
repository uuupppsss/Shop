using Microsoft.AspNetCore.SignalR;

namespace ApiShop.Hubs
{
    public class AdminNoteHub:Hub
    {
        public async Task OrderCreated()
        {
            await Clients.All.SendAsync("OrderCreated");
        }

    }
}
