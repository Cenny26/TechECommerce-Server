using Microsoft.EntityFrameworkCore;
using TechECommerceServer.Domain.Entities;

namespace TechECommerceServer.Persistence.Contexts
{
    public class TechECommerceServerDbContext : DbContext
    {
        protected TechECommerceServerDbContext()
        {
        }
        public TechECommerceServerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
