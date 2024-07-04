using Microsoft.AspNetCore.Builder;
using TechECommerceServer.SignalR.Hubs.Product;

namespace TechECommerceServer.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/products-hub");
        }
    }
}
