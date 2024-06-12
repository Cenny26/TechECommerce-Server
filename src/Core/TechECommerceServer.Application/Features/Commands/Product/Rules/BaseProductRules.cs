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

        public Task GivenProductMustBeLoad(string id, Domain.Entities.Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product), $"The product with the given identity: '{id}' was not found, check it out!");

            return Task.CompletedTask;
        }

        public Task GivenProductsMustBeLoad(IList<Domain.Entities.Product> products)
        {
            if (!products.Any())
                throw new ArgumentNullException(nameof(products), "There is currently no product information in the storage!");

            return Task.CompletedTask;
        }
    }
}
