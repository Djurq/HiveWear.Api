using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HiveWear.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IClothingRepository, ClothingRepository>();
            return services;
        }
    }
}
