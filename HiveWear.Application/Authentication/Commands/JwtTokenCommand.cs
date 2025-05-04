using HiveWear.Application.Interfaces.Providers;
using HiveWear.Application.Interfaces.Services;
using MediatR;

namespace HiveWear.Application.Authentication.Commands
{
    public record class JwtTokenCommand : IRequest<string?>
    {
    }

    public class JwtTokenCommandHandler(IJwtTokenService jwtTokenService, IUserProvider userProvider) : IRequestHandler<JwtTokenCommand, string?>
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        private readonly IUserProvider _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        public Task<string?> Handle(JwtTokenCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException("Request is null", nameof(request));
            }

            string? userId = _userProvider.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("User ID is null", nameof(userId));
            }

            string? token = _jwtTokenService.GenerateJwtToken(userId);

            return Task.FromResult(token);
        }
    }
}
