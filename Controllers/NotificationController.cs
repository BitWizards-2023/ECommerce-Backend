using System.Threading.Tasks;
using ECommerceBackend.Helpers.utills;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly FirebaseUtils _firebaseUtils;

        public NotificationController(FirebaseUtils firebaseUtils)
        {
            _firebaseUtils = firebaseUtils;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(string token, string title, string body)
        {
            if (
                string.IsNullOrEmpty(token)
                || string.IsNullOrEmpty(title)
                || string.IsNullOrEmpty(body)
            )
            {
                return BadRequest("Token, title, and body must be provided.");
            }

            try
            {
                var result = await _firebaseUtils.SendNotificationAsync(token, title, body);
                return Ok(new { message = "Notification sent successfully", result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
