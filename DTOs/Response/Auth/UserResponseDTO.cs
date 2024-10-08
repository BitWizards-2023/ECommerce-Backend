/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the UserResponseDTO, which is used to return user details
 * in response to authentication or user-related API requests. It includes properties for
 * ID, username, email, personal details, role, profile picture, and FCM token.
 * Date Created: 2024/09/28
 */

using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.DTOs.Response.Auth
{
    /// <summary>
    /// Represents the details of a user returned in response to user-related API requests.
    /// </summary>
    public class UserResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
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
        /// Gets or sets the address details of the user.
        /// </summary>
        public AddressResponseDTO Address { get; set; }

        /// <summary>
        /// Gets or sets the date and time the user was created (default is UtcNow).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time the user was last updated (default is UtcNow).
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the role of the user (e.g., "Customer", "Admin").
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the profile picture URL of the user.
        /// </summary>
        public string Profile_pic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Firebase Cloud Messaging (FCM) token for notifications.
        /// </summary>
        public string FcmToken { get; set; } = string.Empty;
    }
}
