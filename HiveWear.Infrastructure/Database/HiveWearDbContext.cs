using HiveWear.Domain.Models;
using HiveWear.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HiveWear.Infrastructure.Database
{
    public class HiveWearDbContext(DbContextOptions<HiveWearDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<ClothingItem> ClothingItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<EntityEntry> entries = [.. ChangeTracker.Entries().Where(e => e.Entity is ClothingItem)];

            foreach (EntityEntry entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((ClothingItem)entry.Entity).DateCreated = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    ((ClothingItem)entry.Entity).DateUpdated = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
