using HiveWear.Domain.Models;

namespace HiveWear.Domain.Interfaces.Repositories
{
    public interface IClothingRepository
    {
        Task<IEnumerable<ClothingItem>> GetAllAsync();
    }
}
