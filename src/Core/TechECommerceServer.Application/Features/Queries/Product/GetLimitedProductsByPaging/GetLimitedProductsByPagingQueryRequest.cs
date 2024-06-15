using MediatR;

namespace TechECommerceServer.Application.Features.Queries.Product.GetLimitedProductsByPaging
{
    public class GetLimitedProductsByPagingQueryRequest : IRequest<GetLimitedProductsByPagingQueryResponse>
    {
        // todo: directly define 'pagination class' as a property!
        public int CurrentPage { get; set; } = 0;
        public int PageSize { get; set; } = 5;
    }
}
