using HiveWear.Domain.Enums;

namespace HiveWear.Domain.Models
{
    public sealed class ClothingItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public Season? Season { get; set; }
    }
}
