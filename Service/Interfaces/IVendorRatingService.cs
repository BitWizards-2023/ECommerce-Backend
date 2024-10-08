/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the IVendorRatingService interface, which provides functionality
 * for managing vendor ratings in the e-commerce system. It includes methods for submitting,
 * approving ratings, and retrieving vendor profiles with rating details.
 * Date Created: 2024/09/28
 */

using ECommerceBackend.DTOs.Request.SubmitRatings;
using ECommerceBackend.DTOs.Response.Vendor;

namespace ECommerceBackend.Services.Interfaces
{
    public interface IVendorRatingService
    {
        /// <summary>
        /// Submits a rating and comment for a specific vendor.
        /// </summary>
        /// <param name="vendorId">The ID of the vendor being rated</param>
        /// <param name="ratingDTO">The rating and comment details</param>
        /// <param name="customerId">The ID of the customer submitting the rating</param>
        /// <returns>True if the rating was submitted successfully, false otherwise</returns>
        Task<bool> SubmitRatingAsync(
            string vendorId,
            SubmitRatingRequestDTO ratingDTO,
            string customerId
        );

        /// <summary>
        /// Approves a rating for a vendor, making it visible to others.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to be approved</param>
        /// <returns>True if the rating was approved successfully, false otherwise</returns>
        Task<bool> ApproveRatingAsync(string ratingId);

        /// <summary>
        /// Retrieves the profile of a vendor, including their ratings and comments.
        /// </summary>
        /// <param name="vendorId">The ID of the vendor</param>
        /// <returns>A VendorProfileResponseDTO containing the vendor's profile and rating details</returns>
        Task<VendorProfileResponseDTO> GetVendorProfileAsync(string vendorId);
    }
}
