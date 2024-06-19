using MediatR;

namespace TechECommerceServer.Application.Features.Queries.ProductImage.GetMatchedImagesByProductId
{
    public class GetMatchedImagesByProductIdQueryRequest : IRequest<List<GetMatchedImagesByProductIdQueryResponse>>
    {
        public string Id { get; set; }
    }
}
