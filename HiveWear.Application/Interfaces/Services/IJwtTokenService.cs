using HiveWear.Domain.Entities;

namespace HiveWear.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string? GenerateJwtToken(string userId);

        RefreshToken GenerateRefreshToken(string userId);
    }
}
