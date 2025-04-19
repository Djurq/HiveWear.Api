using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class LoginCommand(LoginModel? LoginModel) : IRequest<string>
    {
    }

    public class LoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<LoginCommand, string>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginModel is null)
            {
                throw new ArgumentNullException("Login model is null", nameof(request.LoginModel));
            }
            return await _authenticationService.LoginAsync(request.LoginModel).ConfigureAwait(false);
        }
    }
}
