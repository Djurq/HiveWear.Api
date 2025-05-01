using HiveWear.Domain.Models;
using HiveWear.Domain.Models.Authentication;
using HiveWear.Domain.Result;

namespace HiveWear.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<RegisterResult> RegisterAsync(RegisterModel registerModel);

        Task<LoginResult> LoginAsync(LoginModel loginModel);
    }
}
