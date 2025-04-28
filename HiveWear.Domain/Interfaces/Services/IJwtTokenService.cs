using HiveWear.Domain.Models;

namespace HiveWear.Domain.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string? GenerateJwtToken();

        RefreshToken GenerateRefreshToken();
    }
}
