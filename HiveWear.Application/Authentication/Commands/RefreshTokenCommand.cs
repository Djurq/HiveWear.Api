using HiveWear.Application.Authentication.Responses;
using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Application.Interfaces.Services;
using HiveWear.Domain.Entities;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class RefreshTokenCommand(string refreshToken) : IRequest<RefreshTokenResponse>
    {
    }

    public class RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService) : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        private readonly IJwtTokenService jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.refreshToken))
            {
                throw new ArgumentNullException("Refresh token is null", nameof(request.refreshToken));
            }

            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(request.refreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.IsExpired)
            {
                return new RefreshTokenResponse { IsSuccess = false };
            }

            await _refreshTokenRepository.RevokeTokenAsync(storedToken.Token, storedToken.UserId);

            RefreshToken newRefreshToken = jwtTokenService.GenerateRefreshToken(storedToken.UserId);

            bool refreshTokenAdded = await _refreshTokenRepository.AddRefreshTokenAsync(newRefreshToken).ConfigureAwait(false);

            if (!refreshTokenAdded)
            {
                return new RefreshTokenResponse { IsSuccess = false };
            }

            storedToken.ReplacedByToken = newRefreshToken.Token;
            bool replacedByUpdated = await _refreshTokenRepository.UpdateRefreshTokenAsync(storedToken).ConfigureAwait(false);

            if (!replacedByUpdated)
            {
                return new RefreshTokenResponse { IsSuccess = false };
            }

            return new RefreshTokenResponse
            {
                IsSuccess = true,
                RefreshToken = newRefreshToken,
            };
        }
    }
}
