using HiveWear.Domain.Models;

namespace HiveWear.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(RegisterModel registerModel);
        Task<bool> LoginAsync(LoginModel loginModel);
    }
}
