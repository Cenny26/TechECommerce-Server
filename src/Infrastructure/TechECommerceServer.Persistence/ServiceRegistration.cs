using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Persistence.Concretes.Repositories.Product;
using TechECommerceServer.Persistence.Configurations;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<TechECommerceServerDbContext>(options =>
            {
                options.UseNpgsql(DefaultDbConnectionStringConfiguration.ConnectionString);
            });

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
        }
    }
}
