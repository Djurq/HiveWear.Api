using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Infrastructure.Database;
using HiveWear.Infrastructure.Repositories;
using HiveWear.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HiveWear.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IClothingRepository, ClothingRepository>();
            services.AddDatabase();
            services.AddServices();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            string databasePath = "C:\\Users\\Djurr\\source\\repos\\HiveWear.Api\\HiveWear.Infrastructure\\app.db";

            services.AddDbContext<HiveWearDbContext>(options => options.UseSqlite($"Data Source={databasePath}"));

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IFileStorageService, FileStorageService>();
            return services;
        }
    }
}
