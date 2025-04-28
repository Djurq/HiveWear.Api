using HiveWear.Domain.Interfaces.Provider;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace HiveWear.Infrastructure.Provider
{
    internal sealed class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        public string? GetUserId()
        {
            string? userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return userIdClaim;
        }
    }
}
