using HiveWear.Domain.Dto;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class LoginCommand(LoginModel? LoginModel) : IRequest<LoginResult>
    {
    }

    public class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginModel is null)
            {
                throw new ArgumentNullException("Login model is null", nameof(request.LoginModel));
            }

            string loginResult = await _authenticationService.LoginAsync(request.LoginModel).ConfigureAwait(false);

            return new LoginResult(loginResult, request.LoginModel.Email);
        }
    }
}
