/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the CancelOrderDTO, which is used to capture
 * the reason for canceling an order in the e-commerce system.
 * Date Created: 2024/09/28
 */

using System;

namespace ECommerceBackend.DTOs.Request.Order
{
    /// <summary>
    /// Represents the details required to cancel an order, including the reason for cancellation.
    /// </summary>
    public class CancelOrderDTO
    {
        /// <summary>
        /// Gets or sets the reason for canceling the order.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
