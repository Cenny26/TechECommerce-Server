using MediatR;

namespace TechECommerceServer.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandRequest : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
