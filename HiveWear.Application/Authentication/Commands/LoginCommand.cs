using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using HiveWear.Domain.Models.Authentication;
using HiveWear.Domain.Result;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class LoginCommand(LoginModel? LoginModel) : IRequest<LoginResult>
    {
    }

    public class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

        public Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginModel is null)
            {
                throw new ArgumentNullException("Login model is null", nameof(request.LoginModel));
            }

            return _authenticationService.LoginAsync(request.LoginModel);
        }
    }
}
