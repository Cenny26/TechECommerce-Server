namespace TechECommerceServer.Application.Helpers.Product
{
    public static class ProductPrice
    {
        public static decimal CalculateDiscountedPrice(decimal price, decimal? discount)
            => discount.HasValue && discount.Value > 0 ? price - (price * discount.Value / 100) : price;
    }
}
