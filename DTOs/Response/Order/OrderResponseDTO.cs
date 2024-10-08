/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the OrderResponseDTO, which is used to return
 * the details of an order in response to order-related API requests. It includes
 * properties for order ID, order number, total amount, status, shipping address,
 * payment status, creation date, and a list of ordered items.
 * Date Created: 2024/09/28
 */

using System;
using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Order
{
    /// <summary>
    /// Represents the details of an order returned in response to order-related API requests.
    /// </summary>
    public class OrderResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the order number, a unique reference for the order.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the total amount for the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order (e.g., "Pending", "Shipped").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the shipping address for the order.
        /// </summary>
        public AddressDTO ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the payment status of the order (e.g., "Paid", "Pending").
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the list of items included in the order.
        /// </summary>
        public List<OrderItemDTO> Items { get; set; }
    }
}
