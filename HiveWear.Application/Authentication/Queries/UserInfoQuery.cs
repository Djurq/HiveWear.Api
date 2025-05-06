using HiveWear.Application.Authentication.Responses;
using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Application.Interfaces.Services;
using HiveWear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HiveWear.Application.Authentication.Queries
{
    public record class UserInfoQuery(string refreshToken) : IRequest<UserInfoResponse>
    {
    }

    public class UserInfoQueryHandler(
        IRefreshTokenRepository refreshTokenRepository, 
        IJwtTokenService jwtTokenService,
        UserManager<User> userManager) : IRequestHandler<UserInfoQuery, UserInfoResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        public async Task<UserInfoResponse> Handle(UserInfoQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.refreshToken))
            {
                throw new ArgumentException("Refresh token cannot be null or empty.", nameof(request.refreshToken));
            }

            RefreshToken? refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.refreshToken).ConfigureAwait(false);

            if (refreshToken == null)
            {
                throw new InvalidOperationException("Refresh token not found.");
            }

            if (refreshToken.IsExpired)
            {
                throw new InvalidOperationException("Refresh token is expired.");
            }

            if (refreshToken.IsRevoked)
            {
                throw new InvalidOperationException("Refresh token is revoked.");
            }

            string? jwtToken = _jwtTokenService.GenerateJwtToken(refreshToken.UserId);

            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new InvalidOperationException("Failed to generate JWT token.");
            }

            User? User = await _userManager.FindByIdAsync(refreshToken.UserId).ConfigureAwait(false);

            if (User == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return new UserInfoResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token,
                UserName = User.Email ?? string.Empty,
            };
        }
    }
}