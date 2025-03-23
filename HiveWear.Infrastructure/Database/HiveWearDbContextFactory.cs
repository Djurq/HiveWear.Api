using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace HiveWear.Infrastructure.Database
{
    public sealed class HiveWearDbContextFactory : IDesignTimeDbContextFactory<HiveWearDbContext>
    {
        public HiveWearDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<HiveWearDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source=app.db");

            return new HiveWearDbContext(optionsBuilder.Options);
        }
    }
}
