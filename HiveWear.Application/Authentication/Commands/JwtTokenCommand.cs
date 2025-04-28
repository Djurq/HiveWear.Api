using HiveWear.Domain.Interfaces.Services;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class JwtTokenCommand : IRequest<string?>
    {
    }

    public class JwtTokenCommandHandler(IJwtTokenService jwtTokenService) : IRequestHandler<JwtTokenCommand, string?>
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));

        public Task<string?> Handle(JwtTokenCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException("Request is null", nameof(request));
            }

            string? token = _jwtTokenService.GenerateJwtToken();

            return Task.FromResult(token);
        }
    }
}
