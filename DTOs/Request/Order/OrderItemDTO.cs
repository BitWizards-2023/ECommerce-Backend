/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the OrderItemDTO, which is used to capture the details
 * of an individual item in an order, including the product ID and quantity.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Order
{
    /// <summary>
    /// Represents an individual item in an order.
    /// </summary>
    public class OrderItemDTO
    {
        /// <summary>
        /// Gets or sets the ID of the product being ordered.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product being ordered.
        /// </summary>
        public int Quantity { get; set; }
    }
}
