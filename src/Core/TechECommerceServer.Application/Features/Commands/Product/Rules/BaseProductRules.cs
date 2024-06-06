using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Exceptions;

namespace TechECommerceServer.Application.Features.Commands.Product.Rules
{
    public class BaseProductRules : BaseRule
    {
        public Task ProductPriceAndDiscountValuesMustBeValid(decimal price, decimal discount)
        {
            decimal discountPrice = price - (price * discount / 100);
            if (discountPrice < 0)
                throw new ProductPriceAndDiscountValueMustNotBeInvalidException();

            return Task.CompletedTask;
        }
    }
}
