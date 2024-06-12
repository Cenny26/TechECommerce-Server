using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdQueryHandler : BaseHandler, IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly BaseProductRules _productRules;
        public GetProductByIdQueryHandler(IMapper _mapper, IProductReadRepository productReadRepository, BaseProductRules productRules) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
            _productRules = productRules;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id, isTracking: false);
            await _productRules.GivenProductMustBeLoad(request.Id, product);

            GetProductByIdQueryResponse response = _mapper.Map<GetProductByIdQueryResponse>(product);
            return response;
        }
    }
}
