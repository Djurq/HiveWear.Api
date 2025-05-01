using HiveWear.Domain.Interfaces.Provider;
using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using HiveWear.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtConstants = HiveWear.Domain.Constants.JwtConstants;

namespace HiveWear.Infrastructure.Services
{
    internal sealed class JwtTokenService(
        IConfiguration configuration, 
        IUserProvider userProvider,
        UserManager<User> userManager) : IJwtTokenService
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private readonly IUserProvider _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        public string GenerateJwtToken()
        {
            string userId = _userProvider.GetUserId() ?? throw new UnauthorizedAccessException("User ID cannot be null or empty.");

            User? user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new ArgumentNullException("User cannot be null.", nameof(user));
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(user));
            }

            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentException("User name cannot be null or empty.", nameof(user));
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("User email cannot be null or empty.", nameof(user));
            }

            Claim[] claims =
            [
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, user.Email)
            ];

            string secretKey = _configuration[JwtConstants.SecretKey] ?? throw new ArgumentNullException("Secret key cannot be null or empty.", nameof(_configuration));

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                _configuration[JwtConstants.Issuer],
                _configuration[JwtConstants.Audience],
                claims,
                expires: DateTime.UtcNow.Add(JwtConstants.Expiration),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            string token = GenerateRandomString();
            string userId = _userProvider.GetUserId() ?? throw new ArgumentNullException("User ID cannot be null or empty.", nameof(_userProvider));

            RefreshToken refreshToken = new()
            {
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(JwtConstants.Expiration),
            };

            return refreshToken;
        }

        private static string GenerateRandomString()
        {
            byte[] randomNumber = new byte[32];

            string token = string.Empty;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            return token;
        }
    }
}
