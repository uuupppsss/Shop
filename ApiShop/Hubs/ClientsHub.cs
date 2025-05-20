using Microsoft.AspNetCore.SignalR;

namespace ApiShop.Hubs
{
    public class ClientsHub:Hub
    {
        public async Task ConnectClient(int user_id)
        {

        }

        public async Task OrderCreated(int orderId)
        {

        }
        
        public async Task OrderCanseled(int order_id)
        {

        }
    }
}
