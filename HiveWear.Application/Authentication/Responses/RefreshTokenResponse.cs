using HiveWear.Domain.Entities;

namespace HiveWear.Application.Authentication.Responses
{
    public sealed class RefreshTokenResponse
    {
        public RefreshToken RefreshToken { get; set; } = null!;
        public bool IsSuccess { get; set; }
    }
}
