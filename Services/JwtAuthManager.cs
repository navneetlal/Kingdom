using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using KingdomApi.Models;

namespace KingdomApi.Services
{
    public class JwtAuthManager
    {
        public static string GenerateToken(Nobleman nobleman)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, nobleman.Username),
                new Claim(JwtRegisteredClaimNames.Iss, Environment.GetEnvironmentVariable("Jwt:Issuer")),
                new Claim(JwtRegisteredClaimNames.Aud, nobleman.Kingdom.KingdomName),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()),
            };

            var token = new JwtSecurityToken(Environment.GetEnvironmentVariable("Jwt:Issuer"),
              Environment.GetEnvironmentVariable("Jwt:Issuer"),
              claims,
              expires: DateTime.Now.AddMinutes(30),
              notBefore: DateTime.Now.AddSeconds(-5),
              signingCredentials: credentials);

            token.Payload["scope"] = new { role = "h" };

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
