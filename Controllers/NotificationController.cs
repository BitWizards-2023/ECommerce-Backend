/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the NotificationController class,
 * which provides functionality for sending notifications using Firebase Cloud Messaging
 * for the ECommerceBackend application. It includes methods to send notifications with
 * a given token, title, and body.
 * Date Created: 2024/09/18
 */

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

        /// <summary>
        /// Constructor for NotificationController, injecting FirebaseUtils.
        /// </summary>
        /// <param name="firebaseUtils">Utility class for Firebase operations</param>
        public NotificationController(FirebaseUtils firebaseUtils)
        {
            _firebaseUtils = firebaseUtils;
        }

        /// <summary>
        /// Sends a notification via Firebase Cloud Messaging.
        /// </summary>
        /// <param name="token">The FCM token of the recipient device</param>
        /// <param name="title">The title of the notification</param>
        /// <param name="body">The body content of the notification</param>
        /// <returns>Returns success or error status</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(string token, string title, string body)
        {
            // Validate if token, title, and body are provided
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
                // Call FirebaseUtils to send the notification
                var result = await _firebaseUtils.SendNotificationAsync(token, title, body);
                return Ok(new { message = "Notification sent successfully", result });
            }
            catch (Exception ex)
            {
                // Handle any errors during the notification sending process
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
