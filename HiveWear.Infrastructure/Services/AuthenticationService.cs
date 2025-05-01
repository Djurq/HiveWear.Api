using HiveWear.Domain.Interfaces.Repositories;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using HiveWear.Domain.Models.Authentication;
using HiveWear.Domain.Result;
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

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            ArgumentNullException.ThrowIfNull(loginModel);

            User? user = await _userManager.FindByEmailAsync(loginModel.Email).ConfigureAwait(false);

            if (user is not { })
            {
                throw new UnauthorizedAccessException("Invalid login attempt.");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                RefreshToken refreshToken = _jwtTokenService.GenerateRefreshToken(user.Id);
                string? jwtToken = _jwtTokenService.GenerateJwtToken(user.Id);
                bool refreshResult = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken).ConfigureAwait(false);

                if (!refreshResult || string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Failed to add refresh token.");
                }

                return new LoginResult(refreshToken.Token, jwtToken, loginModel.Email);
            }

            throw new UnauthorizedAccessException("Invalid login attempt.");
        }

        public async Task<RegisterResult> RegisterAsync(RegisterModel registerModel)
        {
            ArgumentNullException.ThrowIfNull(registerModel);

            User user = new()
            {
                UserName = registerModel.Email,
                Email = registerModel.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password).ConfigureAwait(false);
            User? generatedUser = await _userManager.FindByEmailAsync(user.Email).ConfigureAwait(false);

            if (result.Succeeded && generatedUser is not null)
            {
                RefreshToken refreshToken = _jwtTokenService.GenerateRefreshToken(generatedUser.Id);
                string? jwtToken = _jwtTokenService.GenerateJwtToken(generatedUser.Id);
                bool refreshResult = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken).ConfigureAwait(false);

                if (!refreshResult || string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Failed to add refresh token.");
                }

                return new RegisterResult(refreshToken.Token, jwtToken, registerModel.Email);
            }

            throw new InvalidOperationException("User registration failed.");
        }
    }
}
