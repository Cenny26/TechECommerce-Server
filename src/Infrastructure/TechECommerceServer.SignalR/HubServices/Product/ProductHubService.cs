using Microsoft.AspNetCore.SignalR;
using TechECommerceServer.Application.Abstractions.Hubs.Product;
using TechECommerceServer.SignalR.Hubs.Product;

namespace TechECommerceServer.SignalR.HubServices.Product
{
    public class ProductHubService : IProductHubService
    {
        private readonly IHubContext<ProductHub> _hubContext;
        public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task ProductAddedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(method: ReceiveFunctionNames.ProductAddedMessage, message);
        }
    }
}
