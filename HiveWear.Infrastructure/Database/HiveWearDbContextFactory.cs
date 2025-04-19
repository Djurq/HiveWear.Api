using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace HiveWear.Infrastructure.Database
{
    public sealed class HiveWearDbContextFactory : IDesignTimeDbContextFactory<HiveWearDbContext>
    {
        public HiveWearDbContext CreateDbContext(string[] args)
        {
            string databasePath = "C:\\Users\\Djurr\\source\\repos\\HiveWear.Api\\HiveWear.Infrastructure\\app.db";

            DbContextOptionsBuilder<HiveWearDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlite($"Data Source={databasePath}");

            return new HiveWearDbContext(optionsBuilder.Options);
        }
    }
}
