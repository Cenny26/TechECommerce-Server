namespace TechECommerceServer.Application.Abstractions.Hubs.Product
{
    public interface IProductHubService
    {
        Task ProductAddedMessageAsync(string message);
    }
}
