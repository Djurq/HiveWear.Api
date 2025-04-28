using HiveWear.Domain.Models;

namespace HiveWear.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterModel registerModel);

        Task<string> LoginAsync(LoginModel loginModel);
    }
}
