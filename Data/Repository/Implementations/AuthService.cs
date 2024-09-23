using ECommerceBackend.Models;
using ECommerceBackend.Utilities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceBackend.Data.Contexts;

namespace ECommerceBackend.Services
{
    using System.Security.Cryptography;

public class AuthService : IAuthService
{
    private readonly MongoDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(MongoDbContext context, IConfiguration configuration)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string Authenticate(string Email, string password, out string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Email and password must be provided.");

        var user = _context.Users.Find(u => u.Email == Email).FirstOrDefault();

        if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var jwtToken = GenerateJwtToken(user);
        refreshToken = GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set refresh token expiry time, e.g., 7 days
        _context.Users.ReplaceOne(u => u.Id == user.Id, user);

        return jwtToken;
    }

    private string GenerateJwtToken(User user)
    {
        var secret = _configuration["JwtSettings:Secret"];
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];
        var tokenLifetimeStr = _configuration["JwtSettings:TokenLifetime"];

        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) ||
            string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(tokenLifetimeStr))
        {
            throw new Exception("JWT settings are not properly configured.");
        }

        if (!TimeSpan.TryParse(tokenLifetimeStr, out var tokenLifetime))
        {
            throw new Exception("Invalid TokenLifetime format in JWT settings.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            }),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public string RefreshToken(string token, string refreshToken)
    {
        var user = _context.Users.Find(u => u.RefreshToken == refreshToken).FirstOrDefault();

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var newJwtToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set new refresh token expiry time
        _context.Users.ReplaceOne(u => u.Id == user.Id, user);

        return newJwtToken;
    }

    public bool Register(string Email, string password)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Email and password must be provided.");

        var existingUser = _context.Users.Find(u => u.Email == Email).FirstOrDefault();
        
        if (existingUser != null)
        {
            return false;
        }

        var newUser = new User
        {
            Email = Email,
            PasswordHash = PasswordHasher.HashPassword(password),
        };

        _context.Users.InsertOne(newUser);
        return true;
    }
       public void Logout(string email)
    {
        var user = _context.Users.Find(u => u.Email == email).FirstOrDefault();

        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue; 
            _context.Users.ReplaceOne(u => u.Id == user.Id, user);
        }
    }
}

}
