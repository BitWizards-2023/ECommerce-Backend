using ECommerceBackend.DTOs.Request;
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
            _authService = authService;
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
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new { token, refreshToken });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            var success = _authService.Register(
                model.Email,
                model.Password,
                model.Username,
                model.FirstName,
                model.LastName,
                model.Role,
                model.Address,
                model.PhoneNumber
            );

            if (!success)
            {
                return BadRequest(new { message = "Email or Username is already taken" });
            }

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
        {
            var newJwtToken = _authService.RefreshToken(model.Token, model.RefreshToken);

            if (newJwtToken == null)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            return Ok(new { token = newJwtToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest model)
        {
            _authService.Logout(model.Email);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset([FromBody] RequestPasswordResetRequest model)
        {
            var resetToken = _authService.RequestPasswordReset(model.Email);
            return Ok(new { message = "Password reset token generated successfully", resetToken });
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
                return BadRequest(new { message = "Invalid or expired password reset token" });
            }

            return Ok(new { message = "Password reset successfully" });
        }
    }
}
