using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechECommerceServer.Application.Abstractions.Storage;
using TechECommerceServer.Domain.Enums;
using TechECommerceServer.Infrastructure.Services.Storage;
using TechECommerceServer.Infrastructure.Services.Storage.Local;

namespace TechECommerceServer.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Configure AutoMapper
            services.AddAutoMapper(assembly);

            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            // todo: case Azure need to scoped for AzureStorage!
            switch (storageType)
            {
                case StorageType.LocalStorage:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
