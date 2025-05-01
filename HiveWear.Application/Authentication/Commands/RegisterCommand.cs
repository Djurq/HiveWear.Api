using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models.Authentication;
using HiveWear.Domain.Result;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class RegisterCommand(RegisterModel? RegisterModel) : IRequest<RegisterResult>
    {
    }

    public class RegisterCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        public Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.RegisterModel is null)
            {
                throw new ArgumentNullException("Register model is null", nameof(request.RegisterModel));
            }

            return _authenticationService.RegisterAsync(request.RegisterModel);
        }
    }
}
