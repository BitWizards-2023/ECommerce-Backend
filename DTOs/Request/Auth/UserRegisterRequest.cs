/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the UserRegisterRequest DTO, which is used to
 * capture the necessary details for registering a new user in the system. It includes
 * properties for username, email, password, personal details, role, profile picture,
 * and Firebase Cloud Messaging (FCM) tokens.
 * Date Created: 2024/09/28
 */

using System;
using ECommerceBackend.DTOs.Request.Auth;

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class UserRegisterRequest
    {
        /// <summary>
        /// Gets or sets the password for the user being registered.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the username for the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address for the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public AddressRequest Address { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user was created (default is UtcNow).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the user was last updated (default is UtcNow).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the role of the user (e.g., "Customer").
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the user's profile picture.
        /// </summary>
        public string Profile_pic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Firebase Cloud Messaging (FCM) tokens for notifications.
        /// </summary>
        public string FcmTokens { get; set; } = string.Empty;
    }
}
