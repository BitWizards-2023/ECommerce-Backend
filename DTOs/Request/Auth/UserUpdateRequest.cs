/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the UserUpdateRequest DTO, which is used to
 * capture the details required to update a user's profile, including personal details,
 * address, and Firebase Cloud Messaging (FCM) token.
 * Date Created: 2024/09/28
 */

using System;
using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class UserUpdateRequest
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public AddressRequest Address { get; set; }

        /// <summary>
        /// Gets or sets the Firebase Cloud Messaging (FCM) token for notifications.
        /// </summary>
        public string FcmToken { get; set; } = string.Empty;
    }
}
