using HiveWear.Domain.Models;

namespace HiveWear.Domain.Interfaces.Repositories
{
    public interface IClothingRepository
    {
        Task<IEnumerable<ClothingItem>> GetAllClothingItemsAsync();
        Task<ClothingItem> AddClothingItemAsync(ClothingItem clothingItem);
        Task<ClothingItem> UpdateClothingItemAsync(ClothingItem clothingItem);
        Task<bool> DeleteClothingItemAsync(int id);
        Task<ClothingItem?> GetClothingItemByIdAsync(int id);
    }
}
