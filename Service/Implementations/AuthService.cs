/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the AuthService class, which provides
 * authentication and user management functionality for the ECommerceBackend application.
 * This includes methods for user registration, login, token generation, password reset,
 * and retrieving user details.
 * Date Created: 2024/09/18
 */

using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.Helpers;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Helpers.utills;
using ECommerceBackend.Utilities;
using MongoDB.Driver;

namespace ECommerceBackend.Services
{
    using System.Security.Cryptography;
    using ECommerceBackend.DTOs.Response.Auth;

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
        public string Authenticate(string email, string password, out string refreshToken)
        {
            refreshToken = null;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _context.Users.Find(u => u.Email == email).FirstOrDefault();

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }

            var jwtToken = JwtTokenUtils.GenerateJwtToken(user, _configuration);
            refreshToken = TokenUtils.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.Users.ReplaceOne(u => u.Id == user.Id, user);

            return jwtToken;
        }

        // Refreshes the JWT token using the refresh token
        public string RefreshToken(string token, string refreshToken)
        {
            var user = _context.Users.Find(u => u.RefreshToken == refreshToken).FirstOrDefault();

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            var newJwtToken = JwtTokenUtils.GenerateJwtToken(user, _configuration);
            var newRefreshToken = TokenUtils.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.Users.ReplaceOne(u => u.Id == user.Id, user);

            return newJwtToken;
        }

        // Registers a new user in the system
        public bool Register(RegisterRequest registerRequest)
        {
            if (
                string.IsNullOrWhiteSpace(registerRequest.Email)
                || string.IsNullOrWhiteSpace(registerRequest.Password)
            )
                throw new ArgumentException("Email and password must be provided.");

            var existingUser = _context
                .Users.Find(u =>
                    u.Email == registerRequest.Email || u.Username == registerRequest.Username
                )
                .FirstOrDefault();

            if (existingUser != null)
            {
                return false;
            }

            var newUser = AuthDTOsMapper.ToUserModel(registerRequest);
            newUser.PasswordHash = PasswordHasher.HashPassword(registerRequest.Password);

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

            var resetToken = TokenUtils.GenerateResetToken();
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
