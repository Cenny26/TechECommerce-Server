using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Queries.ProductImage.GetMatchedImagesByProductId
{
    public class GetMatchedImagesByProductIdQueryHandler : BaseHandler, IRequestHandler<GetMatchedImagesByProductIdQueryRequest, List<GetMatchedImagesByProductIdQueryResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IProductReadRepository _productReadRepository;
        private readonly BaseProductRules _productRules;
        public GetMatchedImagesByProductIdQueryHandler(IMapper _mapper, IConfiguration configuration, IProductReadRepository productReadRepository, BaseProductRules productRules) : base(_mapper)
        {
            _configuration = configuration;
            _productReadRepository = productReadRepository;
            _productRules = productRules;
        }

        public async Task<List<GetMatchedImagesByProductIdQueryResponse>?> Handle(GetMatchedImagesByProductIdQueryRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(navigationPropertyPath => navigationPropertyPath.ProductImages).FirstOrDefaultAsync(item => item.Id == Guid.Parse(request.Id));
            await _productRules.GivenProductMustBeLoad(request.Id, product);
            // todo: optionally, an action can be performed for the situation where the number of images is equal to 0!

            // note: the only reason why i send products anonymously is cause i must create a one contract on the client side that is matched with a suitable request
            return product?.ProductImages.Select(entity => new GetMatchedImagesByProductIdQueryResponse()
            {
                Id = entity.Id,
                Path = $"{_configuration["BaseUrls:BaseLocalStorageUrl"]}/{entity.Path}",
                FileName = entity.FileName
            }).ToList();
        }
    }
}
