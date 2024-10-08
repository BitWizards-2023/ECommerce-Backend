/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the UpdateOrderItemStatusDTO, which is used to capture
 * the details required to update the status of an individual item in an order, including
 * the status and an optional tracking number.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Order
{
    /// <summary>
    /// Represents the details required to update the status of an individual item in an order.
    /// </summary>
    public class UpdateOrderItemStatusDTO
    {
        /// <summary>
        /// Gets or sets the new status for the order item (e.g., "Shipped", "Delivered").
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tracking number for the order item, if applicable (optional).
        /// </summary>
        public string? TrackingNumber { get; set; } // Optional
    }
}
