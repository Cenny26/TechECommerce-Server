using Microsoft.Extensions.DependencyInjection;
using TechECommerceServer.Application.Abstractions.Hubs.Product;
using TechECommerceServer.SignalR.HubServices.Product;

namespace TechECommerceServer.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddTransient<IProductHubService, ProductHubService>();
        }
    }
}
