/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the AuthController class, which provides
 * authentication and user management functionality for the ECommerceBackend application.
 * This includes methods for user registration, login, token generation, password reset,
 * and retrieving user details.
 * Date Created: 2024/09/18
 */

using System.Security.Claims;
using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    /// <summary>
    /// This is the controller for handling user authentication-related requests
    /// such as login, registration, token refresh, logout, and user data retrieval.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        // Dependency Injection for authentication services
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor that injects the authentication service.
        /// </summary>
        /// <param name="authService">The authentication service instance</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Handles the login process by validating the user credentials
        /// and generating JWT and refresh tokens.
        /// </summary>
        /// <param name="model">LoginRequest object containing user email and password</param>
        /// <returns>Returns JWT token and refresh token if successful, Unauthorized if failed</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            // Call the authentication service to generate JWT and refresh tokens
            var token = _authService.Authenticate(
                model.Email,
                model.Password,
                out var refreshToken
            );

            // Check if authentication was successful
            if (token == null)
            {
                return Unauthorized(
                    new ResponseDTO<string>(false, "Invalid email or password", null)
                );
            }

            // Return tokens in a response DTO if login is successful
            return Ok(
                new ResponseDTO<object>(true, "Login successful", new { token, refreshToken })
            );
        }

        /// <summary>
        /// Registers a new user into the system.
        /// </summary>
        /// <param name="model">RegisterRequest object containing user details</param>
        /// <returns>Returns success or failure status of the registration</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            // Call the register method from the authentication service
            var success = _authService.Register(model);

            // Check if registration was successful
            if (!success)
            {
                return BadRequest(
                    new ResponseDTO<string>(false, "Email or Username is already taken", null)
                );
            }

            // Return success response if registration is successful
            return Ok(new ResponseDTO<string>(true, "Registration successful", null));
        }

        /// <summary>
        /// Refreshes the JWT token using a valid refresh token.
        /// </summary>
        /// <param name="model">RefreshTokenRequest object containing the tokens</param>
        /// <returns>Returns a new JWT token or Unauthorized if the refresh token is invalid</returns>
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
        {
            // Attempt to refresh the JWT token
            var newJwtToken = _authService.RefreshToken(model.Token, model.RefreshToken);

            // Check if token refresh was successful
            if (newJwtToken == null)
            {
                return Unauthorized(new ResponseDTO<string>(false, "Invalid refresh token", null));
            }

            // Return new JWT token in a response DTO
            return Ok(
                new ResponseDTO<object>(
                    true,
                    "Token refreshed successfully",
                    new { token = newJwtToken }
                )
            );
        }

        /// <summary>
        /// Logs the user out of the system by invalidating the refresh token.
        /// </summary>
        /// <param name="model">LogoutRequest object containing user email</param>
        /// <returns>Returns success status of the logout operation</returns>
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest model)
        {
            // Invalidate the user's refresh token
            _authService.Logout(model.Email);
            return Ok(new ResponseDTO<string>(true, "Logged out successfully", null));
        }

        /// <summary>
        /// Requests a password reset by generating a reset token for the user.
        /// </summary>
        /// <param name="model">RequestPasswordResetRequest object containing user email</param>
        /// <returns>Returns the generated password reset token</returns>
        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset([FromBody] RequestPasswordResetRequest model)
        {
            // Generate password reset token
            var resetToken = _authService.RequestPasswordReset(model.Email);
            return Ok(
                new ResponseDTO<object>(
                    true,
                    "Password reset token generated successfully",
                    new { resetToken }
                )
            );
        }

        /// <summary>
        /// Resets the user's password using the provided reset token and new password.
        /// </summary>
        /// <param name="model">ResetPasswordRequest object containing email, token, and new password</param>
        /// <returns>Returns success or failure status of the password reset operation</returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
        {
            // Attempt to reset the user's password
            var success = _authService.ResetPassword(
                model.Email,
                model.ResetToken,
                model.NewPassword
            );

            // Check if password reset was successful
            if (!success)
            {
                return BadRequest(
                    new ResponseDTO<string>(false, "Invalid or expired password reset token", null)
                );
            }

            // Return success response if password reset is successful
            return Ok(new ResponseDTO<string>(true, "Password reset successfully", null));
        }

        /// <summary>
        /// Retrieves the details of the currently authenticated user.
        /// </summary>
        /// <returns>Returns the user's details or error if not authenticated</returns>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            try
            {
                // Extract user ID from JWT token claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Check if user ID is present
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(false, "User ID is missing from the token", null)
                    );
                }

                // Retrieve user details from the authentication service
                var user = _authService.GetCurrentUser(userId);

                // Check if user exists
                if (user == null)
                {
                    return NotFound(new ResponseDTO<string>(false, "User not found", null));
                }

                // Return user details in a response DTO
                return Ok(
                    new ResponseDTO<UserResponseDTO>(true, "User retrieved successfully", user)
                );
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }
    }
}
