using HiveWear.Application.Authentication.Requests;
using HiveWear.Application.Authentication.Responses;
using HiveWear.Application.Interfaces.Repositories;
using HiveWear.Application.Interfaces.Services;
using HiveWear.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HiveWear.Infrastructure.Services
{
    internal sealed class AuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository) : IAuthenticationService
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            ArgumentNullException.ThrowIfNull(loginRequest);

            User? user = await _userManager.FindByEmailAsync(loginRequest.Email).ConfigureAwait(false);

            if (user is not { })
            {
                throw new UnauthorizedAccessException("Invalid login attempt.");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                RefreshToken refreshToken = _jwtTokenService.GenerateRefreshToken(user.Id);
                string? jwtToken = _jwtTokenService.GenerateJwtToken(user.Id);
                bool refreshResult = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken).ConfigureAwait(false);

                if (!refreshResult || string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Failed to add refresh token.");
                }

                return new LoginResponse(refreshToken.Token, jwtToken, loginRequest.Email);
            }

            throw new UnauthorizedAccessException("Invalid login attempt.");
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            ArgumentNullException.ThrowIfNull(registerRequest);

            User user = new()
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email
            };

            IdentityResult result;
            try
            {
                result = await _userManager.CreateAsync(user, registerRequest.Password).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the user.", ex);
            }

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User registration failed: {errors}");
            }

            User? generatedUser;
            try
            {
                generatedUser = await _userManager.FindByEmailAsync(user.Email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the created user.", ex);
            }

            if (result.Succeeded && generatedUser is not null)
            {
                RefreshToken refreshToken = _jwtTokenService.GenerateRefreshToken(generatedUser.Id);
                string? jwtToken = _jwtTokenService.GenerateJwtToken(generatedUser.Id);
                bool refreshResult = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken).ConfigureAwait(false);

                if (!refreshResult || string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Failed to add refresh token.");
                }

                return new RegisterResponse(refreshToken.Token, jwtToken, registerRequest.Email);
            }

            throw new InvalidOperationException("User registration failed.");
        }
    }
}
