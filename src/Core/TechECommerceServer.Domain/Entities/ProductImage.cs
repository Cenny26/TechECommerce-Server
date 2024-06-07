namespace TechECommerceServer.Domain.Entities
{
    public class ProductImage : File
    {
        public ICollection<Product> Products { get; set; }
    }
}
