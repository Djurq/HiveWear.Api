using HiveWear.Application.Authentication.Requests;
using HiveWear.Application.Authentication.Responses;
using HiveWear.Application.Interfaces.Services;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class LoginCommand(LoginRequest? LoginModel) : IRequest<LoginResponse>
    {
    }

    public class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

        public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginModel is null)
            {
                throw new ArgumentNullException("Login model is null", nameof(request.LoginModel));
            }

            return _authenticationService.LoginAsync(request.LoginModel);
        }
    }
}
