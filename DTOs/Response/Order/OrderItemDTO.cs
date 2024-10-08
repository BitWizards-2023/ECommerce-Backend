/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the OrderItemDTO, which is used to return the details
 * of an individual item in an order, including the product ID, vendor ID, quantity,
 * status, and associated product details.
 * Date Created: 2024/09/28
 */

using System;

namespace ECommerceBackend.DTOs.Response.Order
{
    /// <summary>
    /// Represents an individual item in an order.
    /// </summary>
    public class OrderItemDTO
    {
        /// <summary>
        /// Gets or sets the ID of the product in the order item.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vendor who sells the product.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the order item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the status of the order item (e.g., "Shipped", "Delivered").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the product details associated with the order item.
        /// </summary>
        public ProductDetailsDTO ProductDetails { get; set; }
    }
}
