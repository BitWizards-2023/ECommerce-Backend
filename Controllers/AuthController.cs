using ECommerceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using ECommerceBackend.DTOs.Request;

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
            var token = _authService.Authenticate(model.Email, model.Password, out var refreshToken);

            if (token == null)
            {
                return Unauthorized(new { message = "Email or password is incorrect" });
            }

            return Ok(new { token, refreshToken });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            var success = _authService.Register(model.Email, model.Password);

            if (!success)
            {
                return BadRequest(new { message = "Email is already taken" });
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
    }
}
