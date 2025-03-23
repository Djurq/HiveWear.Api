using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Models;
using HiveWear.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HiveWear.Infrastructure.Repositories
{
    internal sealed class ClothingRepository(HiveWearDbContext dbContext) : IClothingRepository
    {
        private readonly HiveWearDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<ClothingItem> AddClothingItemAsync(ClothingItem clothingItem)
        {
            EntityEntry<ClothingItem> insertedItem = await _dbContext.ClothingItems.AddAsync(clothingItem).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return insertedItem.Entity;
        }

        public async Task<ClothingItem?> DeleteClothingItemAsync(int id)
        {
            ClothingItem? clothingItem = await _dbContext.ClothingItems.FindAsync(id).ConfigureAwait(false);

            if (clothingItem is null)
            {
                //TODO: Log that the item was not found and throw an exception
                return null;
            }

            _dbContext.ClothingItems.Remove(clothingItem);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return clothingItem;
        }

        public async Task<IEnumerable<ClothingItem>> GetAllClothingItemsAsync()
        {
            return await dbContext.ClothingItems.ToListAsync();
        }

        public async Task<ClothingItem?> GetClothingItemByIdAsync(int id)
        {
            return await dbContext.ClothingItems.FindAsync(id);
        }

        public async Task<ClothingItem> UpdateClothingItemAsync(ClothingItem clothingItem)
        {
            _dbContext.ClothingItems.Update(clothingItem);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return clothingItem;
        }
    }
}
