using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;
using TechECommerceServer.Domain.DTOs.Products;

namespace TechECommerceServer.Application.Features.Queries.Product.GetLimitedProductsByPaging
{
    public class GetLimitedProductsByPagingQueryHandler : BaseHandler, IRequestHandler<GetLimitedProductsByPagingQueryRequest, GetLimitedProductsByPagingQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly BaseProductRules _productRules;
        public GetLimitedProductsByPagingQueryHandler(IMapper _mapper, IProductReadRepository productReadRepository, BaseProductRules productRules) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
            _productRules = productRules;
        }

        public async Task<GetLimitedProductsByPagingQueryResponse> Handle(GetLimitedProductsByPagingQueryRequest request, CancellationToken cancellationToken)
        {
            int totalProductsCount = await _productReadRepository.CountAsync();

            // note: we need a one contract on client side that must matched with a suitable request.
            IList<Domain.Entities.Product> limitedProducts = await _productReadRepository.GetLimitedByPagingAsync(request.CurrentPage, request.PageSize, enableTracking: false);
            await _productRules.GivenProductsMustBeLoad(limitedProducts);

            IList<ProductsByPagingDto> mappedLimitedProducts = _mapper.Map<IList<ProductsByPagingDto>>(limitedProducts);
            return new GetLimitedProductsByPagingQueryResponse()
            {
                TotalProductsCount = totalProductsCount,
                LimitedProducts = mappedLimitedProducts.ToList()
            };
        }
    }
}
