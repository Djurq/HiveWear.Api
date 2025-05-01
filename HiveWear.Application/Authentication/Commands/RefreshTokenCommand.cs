using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models.Authentication;
using HiveWear.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HiveWear.Application.Authentication.Commands
{
    public record class RefreshTokenCommand(string refreshToken) : IRequest<RefreshTokenResult>
    {
    }

    public class RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService) : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        private readonly IJwtTokenService jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));

        public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.refreshToken))
            {
                throw new ArgumentNullException("Refresh token is null", nameof(request.refreshToken));
            }

            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(request.refreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.IsExpired)
            {
                return new RefreshTokenResult { IsSuccess = false };
            }

            await _refreshTokenRepository.RevokeTokenAsync(storedToken.Token, storedToken.UserId);

            RefreshToken newRefreshToken = jwtTokenService.GenerateRefreshToken(storedToken.UserId);

            bool refreshTokenAdded = await _refreshTokenRepository.AddRefreshTokenAsync(newRefreshToken).ConfigureAwait(false);

            if (!refreshTokenAdded)
            {
                return new RefreshTokenResult { IsSuccess = false };
            }

            storedToken.ReplacedByToken = newRefreshToken.Token;
            bool replacedByUpdated = await _refreshTokenRepository.UpdateRefreshTokenAsync(storedToken).ConfigureAwait(false);

            if (!replacedByUpdated)
            {
                return new RefreshTokenResult { IsSuccess = false };
            }

            return new RefreshTokenResult
            {
                IsSuccess = true,
                RefreshToken = newRefreshToken,
            };
        }
    }
}
