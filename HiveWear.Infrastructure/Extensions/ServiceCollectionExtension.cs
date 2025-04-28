using HiveWear.Domain.Constants;
using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using HiveWear.Infrastructure.Database;
using HiveWear.Infrastructure.Repositories;
using HiveWear.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HiveWear.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IClothingRepository, ClothingRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddDatabase();
            services.AddServices();
            services.AddAuthentication();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            string databasePath = "C:\\Users\\Djurr\\source\\repos\\HiveWear.Api\\HiveWear.Infrastructure\\app.db";

            services.AddDbContext<HiveWearDbContext>(options => options.UseSqlite($"Data Source={databasePath}"));

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
