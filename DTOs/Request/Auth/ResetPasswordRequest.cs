/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the ResetPasswordRequest DTO, which is used to
 * capture the necessary details for resetting a user's password, including the email,
 * reset token, and new password.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user requesting the password reset.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the reset token used to verify the password reset request.
        /// </summary>
        public string ResetToken { get; set; }

        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
