using HiveWear.Domain.Dto;
using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HiveWear.Application.Authentication.Commands
{
    public record class RefreshTokenCommand : IRequest<RefreshTokenResult>
    {
    }

    public class RefreshTokenCommandHandler(
        IAuthenticationService authenticationService,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

        public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            string? refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentNullException("Refresh token is null", nameof(refreshToken));
            }

            RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || storedToken.IsRevoked)
            {
                return new RefreshTokenResult { IsSuccess = false };
            }

            if (storedToken.IsExpired)
            {
                await _refreshTokenRepository.RevokeTokenAsync(storedToken.Token, storedToken.UserId);
                return new RefreshTokenResult { IsSuccess = false };
            }

            if (storedToken.ReplacedByToken != null)
            {
                await _refreshTokenRepository.RevokeTokenAsync(storedToken.ReplacedByToken, storedToken.UserId);
            }

            await _refreshTokenRepository.RevokeTokenAsync(storedToken.Token, storedToken.UserId);

            RefreshTokenResult result = new()
            {
                IsSuccess = true,
                Token = storedToken.Token,
                UserId = storedToken.UserId
            };


            return await _authenticationService.GenerateRefreshTokenAsync(request.UserId).ConfigureAwait(false);
        }
    }
}
