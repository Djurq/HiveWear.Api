using HiveWear.Application.Interfaces.Providers;
using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Application.Interfaces.Services;
using HiveWear.Domain.Constants;
using HiveWear.Domain.Entities;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Infrastructure.Database;
using HiveWear.Infrastructure.Provider;
using HiveWear.Infrastructure.Repositories;
using HiveWear.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            #if DEBUG
                        services.AddDbContext<HiveWearDbContext>(options =>
                            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            #else
                        services.AddDbContext<HiveWearDbContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            #endif

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<HiveWearDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IFileStorageService, FileStorageService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            string? secretKey = configuration[JwtConstants.SecretKey];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Secret key cannot be null or empty.", nameof(configuration));
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                        };
                    });

            return services;
        }
    }
}
