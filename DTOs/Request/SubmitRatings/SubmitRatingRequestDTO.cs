/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the SubmitRatingRequestDTO, which is used to capture
 * the rating and comment details submitted by a customer for a vendor.
 * Date Created: 2024/09/28
 */

using System;

namespace ECommerceBackend.DTOs.Request.SubmitRatings
{
    /// <summary>
    /// Represents the details required to submit a rating and comment for a vendor.
    /// </summary>
    public class SubmitRatingRequestDTO
    {
        /// <summary>
        /// Gets or sets the rating score (e.g., 1 to 5).
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the comment provided by the customer.
        /// </summary>
        public string Comment { get; set; } = string.Empty;
    }
}
