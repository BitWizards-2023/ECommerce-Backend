/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the LoginRequest DTO, which is used to capture the
 * email and password details for user login requests.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log in.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; }
    }
}
