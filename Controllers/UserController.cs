using ECommerceBackend.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ECommerceBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;


        [HttpGet("list")]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUserList();

            if (users == null)
            {
                return NotFound("No users found.");
            }

            var userList = users.Select(user => new 
            {
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Role,
                user.CreatedAt,
                user.UpdatedAt
            });

            return Ok(userList);
        }
    }
}
