/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the AddressRequest DTO, which is used to capture the address
 * details when making authentication-related requests (such as user registration).
 * It includes properties for the street, city, state, postal code, and country.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class AddressRequest
    {
        /// <summary>
        /// Gets or sets the street address of the user.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the city of the user.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state or province of the user.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal code of the user.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country of the user.
        /// </summary>
        public string Country { get; set; }
    }
}
