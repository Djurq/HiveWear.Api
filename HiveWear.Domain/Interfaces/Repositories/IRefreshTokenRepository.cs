using HiveWear.Domain.Models;

namespace HiveWear.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);

        Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken);

        Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);

        Task<bool> RevokeTokenAsync(string token, string userId);
    }
}
