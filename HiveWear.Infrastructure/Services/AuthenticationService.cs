using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HiveWear.Infrastructure.Services
{
    internal sealed class AuthenticationService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IJwtTokenService jwtTokenService) : IAuthenticationService
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));

        public async Task<string> LoginAsync(LoginModel loginModel)
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
                return _jwtTokenService.GenerateToken(user);
            }

            throw new UnauthorizedAccessException("Invalid login attempt.");
        }

        public Task<string> RegisterAsync(RegisterModel registerModel)
        {
            throw new NotImplementedException();
        }
    }
}
