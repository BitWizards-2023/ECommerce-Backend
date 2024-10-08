/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the IAuthService interface, which provides
 * authentication and user management functionality. It includes methods for
 * user authentication, registration, token management, password reset,
 * and retrieving user details.
 * Date Created: 2024/09/28
 */

using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user by their email and password.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <param name="refreshToken">The refresh token generated during authentication</param>
        /// <returns>A JWT token if authentication is successful, null otherwise</returns>
        string Authenticate(string email, string password, out string refreshToken);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerRequest">The registration details</param>
        /// <returns>True if registration was successful, false otherwise</returns>
        bool Register(RegisterRequest registerRequest);

        /// <summary>
        /// Refreshes a user's JWT token using a refresh token.
        /// </summary>
        /// <param name="token">The expired or expiring JWT token</param>
        /// <param name="refreshToken">The refresh token associated with the user</param>
        /// <returns>A new JWT token if the refresh is successful, null otherwise</returns>
        string RefreshToken(string token, string refreshToken);

        /// <summary>
        /// Logs a user out by invalidating their tokens.
        /// </summary>
        /// <param name="email">The email of the user logging out</param>
        void Logout(string email);

        /// <summary>
        /// Requests a password reset for the given email.
        /// </summary>
        /// <param name="email">The email of the user requesting a password reset</param>
        /// <returns>A reset token to allow password reset, or null if the user is not found</returns>
        string RequestPasswordReset(string email);

        /// <summary>
        /// Resets the password for a user if the provided reset token is valid.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="resetToken">The token used for verifying the password reset request</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>True if the password reset was successful, false otherwise</returns>
        bool ResetPassword(string email, string resetToken, string newPassword);

        /// <summary>
        /// Retrieves the current user details based on their user ID.
        /// </summary>
        /// <param name="userId">The ID of the current user</param>
        /// <returns>A UserResponseDTO containing the user's details</returns>
        UserResponseDTO GetCurrentUser(string userId);
    }
}
