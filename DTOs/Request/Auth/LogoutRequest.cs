/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the LogoutRequest DTO, which is used to capture the
 * email of the user attempting to log out of the system.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class LogoutRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log out.
        /// </summary>
        public string Email { get; set; }
    }
}
