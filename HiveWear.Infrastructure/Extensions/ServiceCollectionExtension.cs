using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Infrastructure.Database;
using HiveWear.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HiveWear.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IClothingRepository, ClothingRepository>();
            services.AddDatabase();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<HiveWearDbContext>(options => options.UseSqlite("Data Source=app.db"));

            return services;
        }
    }
}
