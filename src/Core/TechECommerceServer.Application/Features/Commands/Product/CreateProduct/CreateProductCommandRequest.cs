using MediatR;

namespace TechECommerceServer.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandRequest : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
