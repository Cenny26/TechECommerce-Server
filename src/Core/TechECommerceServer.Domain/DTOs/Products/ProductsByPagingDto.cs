namespace TechECommerceServer.Domain.DTOs.Products
{
    public class ProductsByPagingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
