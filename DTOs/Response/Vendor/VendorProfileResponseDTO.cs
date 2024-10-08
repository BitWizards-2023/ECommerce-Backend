/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the VendorProfileResponseDTO, AddressDTO, and VendorRatingResponseDTO,
 * which are used to return details about a vendor's profile, including their address, ratings, and personal information
 * in response to vendor-related API requests.
 * Date Created: 2024/09/28
 */

using System;
using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Vendor
{
    /// <summary>
    /// Represents the profile details of a vendor returned in response to vendor-related API requests.
    /// </summary>
    public class VendorProfileResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the vendor.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Gets or sets the username of the vendor.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the vendor.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name of the vendor.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the vendor.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address of the vendor.
        /// </summary>
        public AddressDTO Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the vendor.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the profile picture URL of the vendor.
        /// </summary>
        public string ProfilePic { get; set; }

        /// <summary>
        /// Gets or sets the average rating of the vendor.
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the total number of reviews the vendor has received.
        /// </summary>
        public int TotalReviews { get; set; }

        /// <summary>
        /// Gets or sets the list of ratings received by the vendor.
        /// </summary>
        public List<VendorRatingResponseDTO> Ratings { get; set; } =
            new List<VendorRatingResponseDTO>();
    }

    /// <summary>
    /// Represents the address details of a vendor.
    /// </summary>
    public class AddressDTO
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

    /// <summary>
    /// Represents a rating provided by a customer for a vendor.
    /// </summary>
    public class VendorRatingResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the customer who provided the rating.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the comment provided by the customer.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the rating score (e.g., 1 to 5) provided by the customer.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the rating was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
