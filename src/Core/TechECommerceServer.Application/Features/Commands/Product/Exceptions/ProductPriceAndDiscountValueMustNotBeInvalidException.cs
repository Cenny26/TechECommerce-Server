using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.Product.Exceptions
{
    public class ProductPriceAndDiscountValueMustNotBeInvalidException : BaseException
    {
        public ProductPriceAndDiscountValueMustNotBeInvalidException() : base("Discounted price is invalid, check it out!")
        {
        }
    }
}
