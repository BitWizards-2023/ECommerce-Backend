/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the RequestPasswordResetRequest DTO, which is used to
 * capture the email of a user requesting a password reset.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class RequestPasswordResetRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user requesting a password reset.
        /// </summary>
        public string Email { get; set; }
    }
}
