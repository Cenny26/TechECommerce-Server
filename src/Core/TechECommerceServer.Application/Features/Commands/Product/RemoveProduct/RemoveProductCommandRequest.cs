using MediatR;

namespace TechECommerceServer.Application.Features.Commands.Product.RemoveProduct
{
    public class RemoveProductCommandRequest : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}
