using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HiveWear.Infrastructure.Services
{
    internal sealed class AuthenticationService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IConfiguration configuration) : IAuthenticationService
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            ArgumentNullException.ThrowIfNull(loginModel);

            User? user = await _userManager.FindByEmailAsync(loginModel.Email).ConfigureAwait(false);

            if (user is null)
            {
                return false;
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {

            }

            return result.Succeeded;
        }

        public Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            throw new NotImplementedException();
        }
    }
}
