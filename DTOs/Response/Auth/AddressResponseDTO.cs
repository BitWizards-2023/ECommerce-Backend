/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the AddressResponseDTO, which is used to return
 * the address details in response to authentication-related requests. It includes
 * properties for street, city, state, postal code, and country.
 * Date Created: 2024/09/28
 */

using System;

namespace ECommerceBackend.DTOs.Response.Auth
{
    /// <summary>
    /// Represents the address details returned in response to authentication-related requests.
    /// </summary>
    public class AddressResponseDTO
    {
        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state or province.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }
    }
}
