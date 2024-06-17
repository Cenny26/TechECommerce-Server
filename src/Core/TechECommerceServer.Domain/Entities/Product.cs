using TechECommerceServer.Domain.Entities.Common;

namespace TechECommerceServer.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product() { }
        public Product(string title, string description, int stockQuantity, decimal price, decimal discount)
        {
            Title = title;
            Description = description;
            StockQuantity = stockQuantity;
            Price = price;
            Discount = discount;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; } = 0;

        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
