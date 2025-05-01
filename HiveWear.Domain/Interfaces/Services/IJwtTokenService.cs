using HiveWear.Domain.Models.Authentication;

namespace HiveWear.Domain.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string? GenerateJwtToken(string userId);

        RefreshToken GenerateRefreshToken(string userId);
    }
}
