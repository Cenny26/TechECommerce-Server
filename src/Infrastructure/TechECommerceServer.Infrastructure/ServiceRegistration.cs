using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechECommerceServer.Application.Abstractions.Cache;
using TechECommerceServer.Application.Abstractions.Mail;
using TechECommerceServer.Application.Abstractions.Storage;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Domain.Enums;
using TechECommerceServer.Infrastructure.Services.Cache;
using TechECommerceServer.Infrastructure.Services.Mail;
using TechECommerceServer.Infrastructure.Services.Storage;
using TechECommerceServer.Infrastructure.Services.Storage.Azure;
using TechECommerceServer.Infrastructure.Services.Storage.Local;
using TechECommerceServer.Infrastructure.Services.Token;

namespace TechECommerceServer.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Configure AutoMapper
            services.AddAutoMapper(assembly);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Cache:DefaultSettings:ConnectionString"];
                options.InstanceName = configuration["Cache:DefaultSettings:InstanceName"];
            });

            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            services.Configure<RedisCacheSettings>(configuration.GetSection("Cache:DefaultSettings"));
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            // done: case Azure need to scoped for AzureStorage!
            switch (storageType)
            {
                case StorageType.LocalStorage:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.AzureStorage:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
