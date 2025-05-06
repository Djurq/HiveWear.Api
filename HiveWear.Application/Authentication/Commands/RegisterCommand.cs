using HiveWear.Application.Authentication.Requests;
using HiveWear.Application.Authentication.Responses;
using HiveWear.Application.Interfaces.Services;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class RegisterCommand(RegisterRequest? RegisterModel) : IRequest<RegisterResponse>
    {
    }

    public class RegisterCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        public Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.RegisterModel is null)
            {
                throw new ArgumentNullException("Register model is null", nameof(request.RegisterModel));
            }

            return _authenticationService.RegisterAsync(request.RegisterModel);
        }
    }
}
