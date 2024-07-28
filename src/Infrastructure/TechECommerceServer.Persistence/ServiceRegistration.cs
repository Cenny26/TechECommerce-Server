using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechECommerceServer.Application.Abstractions.Repositories.File;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Application.Abstractions.Services.AppUser;
using TechECommerceServer.Application.Abstractions.Services.Authentications;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Domain.Entities.Identity;
using TechECommerceServer.Persistence.Concretes.Repositories.File;
using TechECommerceServer.Persistence.Concretes.Repositories.Product;
using TechECommerceServer.Persistence.Concretes.Repositories.ProductImage;
using TechECommerceServer.Persistence.Concretes.Services.AppUser;
using TechECommerceServer.Persistence.Concretes.Services.Authentications.Base;
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
            }).AddEntityFrameworkStores<TechECommerceServerDbContext>().AddDefaultTokenProviders();

            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IExternalAuthenticationService, AuthService>();
            services.AddScoped<IInternalAuthenticationService, AuthService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IProductImageReadRepository, ProductImageReadRepository>();
            services.AddScoped<IProductImageWriteRepository, ProductImageWriteRepository>();
        }
    }
}
