/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the RegisterRequest DTO, which is used to capture
 * the necessary details for user registration, including email, password, username,
 * first name, last name, address, phone number, role, profile picture, and FCM token.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user registering.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the address details of the user.
        /// </summary>
        public AddressRequest Address { get; set; } = new AddressRequest();

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role of the user (default is "Customer").
        /// </summary>
        public string Role { get; set; } = "Customer";

        /// <summary>
        /// Gets or sets the URL for the user's profile picture.
        /// </summary>
        public string Profile_pic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Firebase Cloud Messaging token for notifications.
        /// </summary>
        public string FcmToken { get; set; } = string.Empty;
    }
}
