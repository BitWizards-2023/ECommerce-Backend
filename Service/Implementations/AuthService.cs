/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the AuthService class, which provides
 * authentication and user management functionality for the ECommerceBackend application.
 * This includes methods for user registration, login, token generation, password reset,
 * and retrieving user details.
 * Date Created: 2024/09/18
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.Helpers;
using ECommerceBackend.Models;
using ECommerceBackend.Utilities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ECommerceBackend.Services
{
    using System.Security.Cryptography;
    using ECommerceBackend.DTOs.Response;

    public class AuthService : IAuthService
    {
        private readonly MongoDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor for the AuthService class, initializes the database context and configuration
        public AuthService(MongoDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Authenticates a user based on their email and password, returns a JWT token if successful
        public string Authenticate(string Email, string password, out string refreshToken)
        {
            refreshToken = null;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _context.Users.Find(u => u.Email == Email).FirstOrDefault();

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }

            var jwtToken = GenerateJwtToken(user);
            refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.Users.ReplaceOne(u => u.Id == user.Id, user);

            return jwtToken;
        }

        // Generates a JWT token for the authenticated user
        private string GenerateJwtToken(User user)
        {
            var secret = _configuration["JwtSettings:Secret"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var tokenLifetimeStr = _configuration["JwtSettings:TokenLifetime"];
            Console.WriteLine($"Secret: {secret}");
            Console.WriteLine($"Issuer: {issuer}");
            Console.WriteLine($"Audience: {audience}");
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
                //Issuer = issuer,
                //Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        // Generates a secure refresh token
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Refreshes the JWT token using the refresh token
        public string RefreshToken(string token, string refreshToken)
        {
            var user = _context.Users.Find(u => u.RefreshToken == refreshToken).FirstOrDefault();

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            var newJwtToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.Users.ReplaceOne(u => u.Id == user.Id, user);

            return newJwtToken;
        }

        // Registers a new user in the system
        public bool Register(
            string email,
            string password,
            string username,
            string role,
            string firstName,
            string lastName,
            AddressRequest address,
            string phoneNumber,
            string ProfilePic
        )
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password must be provided.");

            var existingUser = _context
                .Users.Find(u => u.Email == email || u.Username == username)
                .FirstOrDefault();

            if (existingUser != null)
            {
                return false;
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
                Role = string.IsNullOrWhiteSpace(role) ? "Customer" : role,
                ProfilePic = string.IsNullOrWhiteSpace(ProfilePic)
                    ? "https://shopilystorage.blob.core.windows.net/mycontainer/7e888d3f-e276-49c8-bdbc-e5a4fa53a7f0.png"
                    : ProfilePic,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
            };

            _context.Users.InsertOne(newUser);
            return true;
        }

        // Logs out a user by invalidating the refresh token
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

        // Requests a password reset for the user and returns the reset token
        public string RequestPasswordReset(string email)
        {
            var user = _context.Users.Find(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("No user found with the provided email.");
            }

            var resetToken = GenerateResetToken();
            user.PasswordResetToken = resetToken;
            user.ResetTokenExpiryTime = DateTime.UtcNow.AddHours(1);

            _context.Users.ReplaceOne(u => u.Id == user.Id, user);

            return resetToken;
        }

        // Generates a secure reset token
        private string GenerateResetToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Resets the user's password using the reset token
        public bool ResetPassword(string email, string resetToken, string newPassword)
        {
            var user = _context
                .Users.Find(u => u.Email == email && u.PasswordResetToken == resetToken)
                .FirstOrDefault();

            if (user == null || user.ResetTokenExpiryTime <= DateTime.UtcNow)
            {
                return false;
            }

            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpiryTime = DateTime.MinValue;

            _context.Users.ReplaceOne(u => u.Id == user.Id, user);
            return true;
        }

        // Retrieves the details of the currently authenticated user
        public UserResponseDTO GetCurrentUser(string userId)
        {
            var user = _context.Users.Find(u => u.Id == userId).FirstOrDefault();
            if (user == null)
                return null;

            return DtoMapper.ToUserResponseDTO(user);
        }
    }
}
