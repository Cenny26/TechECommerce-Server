using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : BaseHandler, IRequestHandler<UpdateProductCommandRequest, Unit>
    {
        private readonly BaseProductRules _productRules;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        public UpdateProductCommandHandler(IMapper _mapper, BaseProductRules productRules, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository) : base(_mapper)
        {
            _productRules = productRules;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id, isTracking: false);

            await _productRules.GivenProductMustBeLoad(request.Id, product);
            await _productRules.ProductPriceAndDiscountValuesMustBeValid(request.Price, request.Discount);

            Domain.Entities.Product modifiedProduct = _mapper.Map(request, product);

            bool result = _productWriteRepository.Update(entity: modifiedProduct);
            if (result)
            {
                await _productWriteRepository.SaveChangesAsync();
                return Unit.Value;
            }

            // note: throwing an exception if the update fails
            throw new InvalidOperationException("Failed to update the product. The operation did not complete successfully.");
        }
    }
}
