using HiveWear.Domain.Enums;
using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Models;

namespace HiveWear.Infrastructure.Repositories
{
    internal sealed class ClothingRepository : IClothingRepository
    {
        public Task<IEnumerable<ClothingItem>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ClothingItem>>(new List<ClothingItem>
            {
                new ClothingItem
                {
                    Id = 1,
                    Name = "T-shirt",
                    ImageUrl = "https://example.com/t-shirt.jpg",
                    Description = "A simple t-shirt",
                    Category = "Shirts",
                    Color = "White",
                    Size = "M",
                    Brand = "HiveWear",
                    Season = Season.Summer
                },
                new ClothingItem
                {
                    Id = 2,
                    Name = "Sweater",
                    ImageUrl = "https://example.com/sweater.jpg",
                    Description = "A warm sweater",
                    Category = "Sweaters",
                    Color = "Blue",
                    Size = "L",
                    Brand = "HiveWear",
                    Season = Season.Winter
                }
            });
        }
    }
}
