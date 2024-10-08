/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the CreateOrderRequestDTO, which is used to capture
 * the details required for creating a new order in the e-commerce system. It includes
 * a list of order items, the shipping address, and the selected payment method.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Order
{
    /// <summary>
    /// Represents the details required to create a new order in the e-commerce system.
    /// </summary>
    public class CreateOrderRequestDTO
    {
        /// <summary>
        /// Gets or sets the list of items in the order.
        /// </summary>
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();

        /// <summary>
        /// Gets or sets the shipping address for the order.
        /// </summary>
        public AddressDTO ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the payment method for the order (e.g., "Credit Card", "PayPal").
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
