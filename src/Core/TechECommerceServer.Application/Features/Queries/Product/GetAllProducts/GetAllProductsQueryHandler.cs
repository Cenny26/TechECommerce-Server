using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : BaseHandler, IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly BaseProductRules _productRules;
        public GetAllProductsQueryHandler(IMapper _mapper, IProductReadRepository productReadRepository, BaseProductRules productRules) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
            _productRules = productRules;
        }

        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Product> products = await _productReadRepository.GetAllAsync(enableTracking: false);
            await _productRules.GivenProductsMustBeLoad(products);

            IList<GetAllProductsQueryResponse> response = _mapper.Map<IList<GetAllProductsQueryResponse>>(products);
            return response;
        }
    }
}
