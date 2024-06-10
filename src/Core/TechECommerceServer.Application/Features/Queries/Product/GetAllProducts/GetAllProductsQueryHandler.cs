using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : BaseHandler, IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        public GetAllProductsQueryHandler(IMapper _mapper, IProductReadRepository productReadRepository) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Product> products = await _productReadRepository.GetAllAsync(enableTracking: false);

            IList<GetAllProductsQueryResponse> response = _mapper.Map<IList<GetAllProductsQueryResponse>>(products);
            return response;
        }
    }
}
