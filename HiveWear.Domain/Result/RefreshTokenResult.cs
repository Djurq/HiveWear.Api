using HiveWear.Domain.Models.Authentication;

namespace HiveWear.Domain.Result
{
    public sealed class RefreshTokenResult
    {
        public RefreshToken RefreshToken { get; set; } = null!;
        public bool IsSuccess { get; set; }
    }
}
