using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Models.Authentication;
using HiveWear.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HiveWear.Infrastructure.Repositories
{
    internal sealed class RefreshTokenRepository(HiveWearDbContext dbContext) : IRefreshTokenRepository
    {
        private readonly HiveWearDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);

            bool isSaved = await _dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;

            return isSaved;
        }

        public Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return _dbContext.RefreshTokens
                     .AsNoTracking()
                     .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<bool> RevokeTokenAsync(string token, string userId)
        {
            RefreshToken? refreshToken = await _dbContext.RefreshTokens
                                         .FirstOrDefaultAsync(x => x.Token == token && x.UserId == userId)
                                         .ConfigureAwait(false);

            if (refreshToken is null)
            {
                return false;
            }

            refreshToken.RevokedAt = DateTime.UtcNow;
            _dbContext.RefreshTokens.Update(refreshToken);

            bool isSaved = await _dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;

            return isSaved;
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Update(refreshToken);

            bool isSaved = await _dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;

            return isSaved;
        }
    }
}
