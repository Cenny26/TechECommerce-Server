using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechECommerceServer.Application.Abstractions.Repositories.File;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Domain.Entities.Identity;
using TechECommerceServer.Persistence.Concretes.Repositories.File;
using TechECommerceServer.Persistence.Concretes.Repositories.Product;
using TechECommerceServer.Persistence.Concretes.Repositories.ProductImage;
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
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<TechECommerceServerDbContext>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IProductImageReadRepository, ProductImageReadRepository>();
            services.AddScoped<IProductImageWriteRepository, ProductImageWriteRepository>();
        }
    }
}
