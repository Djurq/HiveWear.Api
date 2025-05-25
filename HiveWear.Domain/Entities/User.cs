using Microsoft.AspNetCore.Identity;

namespace HiveWear.Domain.Entities
{
    public sealed class User : IdentityUser
    {
        ICollection<ClothingItem> ClothingItems { get; set; } = [];
    }
}
