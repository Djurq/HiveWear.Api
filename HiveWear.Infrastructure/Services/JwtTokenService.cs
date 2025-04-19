using HiveWear.Domain.Interfaces.Services;
using HiveWear.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HiveWear.Infrastructure.Services
{
    internal sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public string GenerateToken(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

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
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            ];

            string secretKey = _configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Secret key cannot be null or empty.", nameof(_configuration));

            SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials creds = new (key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new (
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
