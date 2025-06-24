using HiveWear.Application.Interfaces.Providers;
using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Application.Interfaces.Services;
using HiveWear.Domain.Constants;
using HiveWear.Domain.Entities;
using HiveWear.Infrastructure.Database;
using HiveWear.Infrastructure.Provider;
using HiveWear.Infrastructure.Repositories;
using HiveWear.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HiveWear.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IClothingRepository, ClothingRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserProvider, UserProvider>();

            services.AddDatabase(configuration);
            services.AddServices();
            services.AddAuthentication();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            
                        services.AddDbContext<HiveWearDbContext>(options =>
                            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            
/*                        services.AddDbContext<HiveWearDbContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));*/
            

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<HiveWearDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
