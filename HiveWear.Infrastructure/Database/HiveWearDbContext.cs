﻿using HiveWear.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HiveWear.Infrastructure.Database
{
    public sealed class HiveWearDbContext(DbContextOptions<HiveWearDbContext> options) : DbContext(options)
    {
        public DbSet<ClothingItem> ClothingItems { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            List<EntityEntry> entries = [.. ChangeTracker.Entries().Where(e => e.Entity is ClothingItem)];

            foreach (var entry in entries)
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

            // Call base SaveChangesAsync
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
