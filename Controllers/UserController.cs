using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        // Ensure there is only one constructor
        public UserController(IUserServices userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("list")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userService.GetUserList();

                if (users == null || !users.Any())
                {
                    return NotFound(
                        new ResponseDTO<List<UserResponseDTO>>(false, "No users found", null)
                    );
                }

                return Ok(
                    new ResponseDTO<List<UserResponseDTO>>(
                        true,
                        "Users retrieved successfully",
                        users
                    )
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
