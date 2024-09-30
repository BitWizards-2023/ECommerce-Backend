/*
 * File: JwtTokenUtils.cs
 * Description: Provides utility methods for generating JWT tokens for users.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-09-29
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceBackend.Models;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceBackend.Helpers.utills
{
    public class JwtTokenUtils
    {
        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="configuration">Application configuration settings.</param>
        /// <returns>A JWT token as a string.</returns>
        // Generates a JWT token for the specified user using the provided configuration settings.
        public static string GenerateJwtToken(User user, IConfiguration configuration)
        {
            var secret = configuration["JwtSettings:Secret"];
            var issuer = configuration["JwtSettings:Issuer"];
            var audience = configuration["JwtSettings:Audience"];
            var tokenLifetimeStr = configuration["JwtSettings:TokenLifetime"];

            if (
                string.IsNullOrEmpty(secret)
                || string.IsNullOrEmpty(issuer)
                || string.IsNullOrEmpty(audience)
                || string.IsNullOrEmpty(tokenLifetimeStr)
            )
            {
                throw new ArgumentException("JWT settings are not properly configured.");
            }

            if (!TimeSpan.TryParse(tokenLifetimeStr, out var tokenLifetime))
            {
                throw new ArgumentException("Invalid TokenLifetime format in JWT settings.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);
            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim(JwtRegisteredClaimNames.Aud, audience),
                        new Claim(JwtRegisteredClaimNames.Iss, issuer),
                    }
                ),
                NotBefore = now,
                IssuedAt = now,
                Expires = now.Add(tokenLifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
