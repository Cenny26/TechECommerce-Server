using MediatR;
using TechECommerceServer.Application.Abstractions.Cache;
using TechECommerceServer.Application.Abstractions.Cache.Utils;

namespace TechECommerceServer.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryRequest : IRequest<IList<GetAllProductsQueryResponse>>, ICacheableQuery
    {
        public string CacheKey => "AllProducts";

        // note: reducing the default cache time to 60 minutes is only valid for the current request.
        public double CacheTime => DefaultCacheVariables.StandardCacheTime;
    }
}
