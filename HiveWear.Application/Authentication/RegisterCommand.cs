using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using MediatR;

namespace HiveWear.Application.Authentication
{
    public record class RegisterCommand(RegisterModel? RegisterModel) : IRequest<string>
    {
    }

    public class RegisterCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RegisterCommand, string>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.RegisterModel is null)
            {
                throw new ArgumentNullException("Register model is null", nameof(request.RegisterModel));
            }

            return await _authenticationService.RegisterAsync(request.RegisterModel).ConfigureAwait(false);
        }
    }
}
