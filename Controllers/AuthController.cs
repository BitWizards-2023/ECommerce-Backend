using System.Security.Claims;
using ECommerceBackend.DTOs.Request;
using ECommerceBackend.DTOs.Response;
using ECommerceBackend.Helpers;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var token = _authService.Authenticate(
                model.Email,
                model.Password,
                out var refreshToken
            );

            if (token == null)
            {
                return Unauthorized(
                    new ResponseDTO<string>(false, "Invalid email or password", null)
                );
            }

            return Ok(
                new ResponseDTO<object>(true, "Login successful", new { token, refreshToken })
            );
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            var success = _authService.Register(
                model.Email,
                model.Password,
                model.Username,
                model.Role,
                model.FirstName,
                model.LastName,
                model.Address,
                model.PhoneNumber,
                model.Profile_pic
            );

            if (!success)
            {
                return BadRequest(
                    new ResponseDTO<string>(false, "Email or Username is already taken", null)
                );
            }

            return Ok(new ResponseDTO<string>(true, "Registration successful", null));
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
        {
            var newJwtToken = _authService.RefreshToken(model.Token, model.RefreshToken);

            if (newJwtToken == null)
            {
                return Unauthorized(new ResponseDTO<string>(false, "Invalid refresh token", null));
            }

            return Ok(
                new ResponseDTO<object>(
                    true,
                    "Token refreshed successfully",
                    new { token = newJwtToken }
                )
            );
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest model)
        {
            _authService.Logout(model.Email);
            return Ok(new ResponseDTO<string>(true, "Logged out successfully", null));
        }

        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset([FromBody] RequestPasswordResetRequest model)
        {
            var resetToken = _authService.RequestPasswordReset(model.Email);
            return Ok(
                new ResponseDTO<object>(
                    true,
                    "Password reset token generated successfully",
                    new { resetToken }
                )
            );
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var success = _authService.ResetPassword(
                model.Email,
                model.ResetToken,
                model.NewPassword
            );

            if (!success)
            {
                return BadRequest(
                    new ResponseDTO<string>(false, "Invalid or expired password reset token", null)
                );
            }

            return Ok(new ResponseDTO<string>(true, "Password reset successfully", null));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(false, "User ID is missing from the token", null)
                    );
                }

                var user = _authService.GetCurrentUser(userId);

                if (user == null)
                {
                    return NotFound(new ResponseDTO<string>(false, "User not found", null));
                }

                return Ok(
                    new ResponseDTO<UserResponseDTO>(true, "User retrieved successfully", user)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }
    }
}
