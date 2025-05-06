using HiveWear.Application.Authentication.Requests;
using HiveWear.Application.Authentication.Responses;

namespace HiveWear.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);

        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    }
}
