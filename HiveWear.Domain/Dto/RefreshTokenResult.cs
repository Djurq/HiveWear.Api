using HiveWear.Domain.Models;

namespace HiveWear.Domain.Dto
{
    public sealed class RefreshTokenResult
    {
        public RefreshToken RefreshToken { get; set; } = null!;
        public bool IsSuccess { get; set; }
    }
}
