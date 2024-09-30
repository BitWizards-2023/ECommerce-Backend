using System.Security.Claims;
using ECommerceBackend.Data.Repository.Interfaces;
using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [Authorize(Policy = "AdminPolicy")]
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            try
            {
                var user = _userService.GetUserById(id);

                if (user == null)
                {
                    return NotFound(
                        new ResponseDTO<UserResponseDTO>(false, "User not found", null)
                    );
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            try
            {
                var result = _userService.UpdateUser(id, userUpdateRequest);
                if (!result)
                {
                    return NotFound(
                        new ResponseDTO<string>(false, "User not found or update failed", null)
                    );
                }

                return Ok(new ResponseDTO<string>(true, "User updated successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                var result = _userService.DeleteUser(id);
                if (!result)
                {
                    return NotFound(
                        new ResponseDTO<string>(false, "User not found or deletion failed", null)
                    );
                }

                return Ok(new ResponseDTO<string>(true, "User deactivated successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}/activate")]
        public IActionResult ActivateUser(string id)
        {
            try
            {
                var result = _userService.ActivateUser(id);

                if (!result)
                {
                    return NotFound(
                        new ResponseDTO<string>(false, "User not found or activation failed", null)
                    );
                }

                return Ok(new ResponseDTO<string>(true, "User activated successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}/approve")]
        public IActionResult ApproveUser(string id)
        {
            try
            {
                var result = _userService.ApproveUser(id);

                if (!result)
                {
                    return NotFound(
                        new ResponseDTO<string>(false, "User not found or approval failed", null)
                    );
                }

                return Ok(new ResponseDTO<string>(true, "User approved successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        [Authorize]
        [HttpPut("update-me")]
        public IActionResult UpdateUserDetails([FromBody] UserUpdateRequest userUpdateDTO)
        {
            try
            {
                // Extract the user ID from the token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ResponseDTO<string>(false, "Invalid token", null));
                }

                var result = _userService.UpdateUser(userId, userUpdateDTO);

                if (!result)
                {
                    return NotFound(
                        new ResponseDTO<string>(false, "User not found or update failed", null)
                    );
                }

                return Ok(new ResponseDTO<string>(true, "User details updated successfully", null));
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
