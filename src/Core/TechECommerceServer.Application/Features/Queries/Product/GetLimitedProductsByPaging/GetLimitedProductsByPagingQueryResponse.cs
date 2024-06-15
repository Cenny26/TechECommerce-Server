namespace TechECommerceServer.Application.Features.Queries.Product.GetLimitedProductsByPaging
{
    public class GetLimitedProductsByPagingQueryResponse
    {
        public int TotalProductsCount { get; set; }
        public object LimitedProducts { get; set; }
    }
}
