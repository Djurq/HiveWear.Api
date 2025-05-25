using HiveWear.Application.Interfaces.Providers;
using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Domain.Entities;
using HiveWear.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HiveWear.Infrastructure.Repositories
{
    internal sealed class ClothingRepository(HiveWearDbContext dbContext, IUserProvider userProvider) : IClothingRepository
    {
        private readonly HiveWearDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUserProvider _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));

        public async Task<ClothingItem> AddClothingItemAsync(ClothingItem clothingItem)
        {
            string? userId = _userProvider.GetUserId() ?? throw new InvalidOperationException("User ID cannot be null");

            clothingItem.UserId = userId;

            EntityEntry<ClothingItem> insertedItem = await _dbContext.ClothingItems.AddAsync(clothingItem).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return insertedItem.Entity;
        }

        public async Task<bool> DeleteClothingItemAsync(int id)
        {
            ClothingItem? clothingItem = await _dbContext.ClothingItems.FindAsync(id).ConfigureAwait(false);

            if (clothingItem is null)
            {
                return false;
            }

            _dbContext.ClothingItems.Remove(clothingItem);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return true;
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
            string? userId = _userProvider.GetUserId() ?? throw new InvalidOperationException("User ID cannot be null");

            ClothingItem? existingItem = await _dbContext.ClothingItems.FindAsync(clothingItem.Id).ConfigureAwait(false) ?? throw new InvalidOperationException($"Clothing item with ID {clothingItem.Id} not found");
            if (existingItem.UserId != userId)
            {
                throw new InvalidOperationException("User ID does not match the clothing item's user ID");
            }

            clothingItem.UserId = userId;

            _dbContext.ClothingItems.Update(clothingItem);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return clothingItem;
        }
    }
}
