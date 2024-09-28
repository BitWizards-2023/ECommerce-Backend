using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Models;
using ECommerceBackend.Utilities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

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
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Authenticate(string Email, string password, out string refreshToken)
        {
            refreshToken = null; // Initialize refreshToken to null

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
                return null; // Email and password are required

            // Retrieve the user from the database
            var user = _context.Users.Find(u => u.Email == Email).FirstOrDefault();

            // Check if the user exists and if the password is valid
            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return null; // Invalid email or password
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
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role ?? "User"),
                    }
                ),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
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

        public bool Register(
            string email,
            string password,
            string username,
            string role,
            string firstName,
            string lastName,
            AddressRequest address,
            string phoneNumber
        )
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password must be provided.");

            var existingUser = _context
                .Users.Find(u => u.Email == email || u.Username == username)
                .FirstOrDefault();

            if (existingUser != null)
            {
                return false; // Email or Username already taken
            }

            var newUser = new User
            {
                Email = email,
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                Address = new Address
                {
                    Street = address?.Street,
                    City = address?.City,
                    State = address?.State,
                    PostalCode = address?.PostalCode,
                    Country = address?.Country,
                    IsDeleted = false,
                },
                PhoneNumber = phoneNumber,
                Role = role ?? "user",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
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

        public string RequestPasswordReset(string email)
        {
            var user = _context.Users.Find(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("No user found with the provided email.");
            }

            // Generate a password reset token
            var resetToken = GenerateResetToken();
            user.PasswordResetToken = resetToken;
            user.ResetTokenExpiryTime = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

            _context.Users.ReplaceOne(u => u.Id == user.Id, user);

            // Ideally, you would send the reset token to the user's email
            return resetToken;
        }

        private string GenerateResetToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public bool ResetPassword(string email, string resetToken, string newPassword)
        {
            var user = _context
                .Users.Find(u => u.Email == email && u.PasswordResetToken == resetToken)
                .FirstOrDefault();

            if (user == null || user.ResetTokenExpiryTime <= DateTime.UtcNow)
            {
                return false; // Invalid token or token expired
            }

            // Update the user's password
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            user.PasswordResetToken = null; // Clear the reset token
            user.ResetTokenExpiryTime = DateTime.MinValue; // Reset expiry time

            _context.Users.ReplaceOne(u => u.Id == user.Id, user);
            return true;
        }
    }
}
