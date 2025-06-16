using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HiveWear.Infrastructure.Database
{
    public class HiveWearDbContextFactory : IDesignTimeDbContextFactory<HiveWearDbContext>
    {
        public HiveWearDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HiveWearDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlite(connectionString);

            return new HiveWearDbContext(optionsBuilder.Options);
        }
    }
}
