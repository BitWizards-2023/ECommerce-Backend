/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the DTOs related to shopping cart operations, including
 * adding or updating cart items, updating the quantity or options of cart items, and applying
 * discount codes to the cart.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Cart
{
    /// <summary>
    /// Represents a request to add or update a cart item in the shopping cart.
    /// </summary>
    public class CartItemRequestDTO
    {
        /// <summary>
        /// Gets or sets the ID of the product being added to the cart.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product to add.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the selected options for the product (e.g., size, color).
        /// </summary>
        public Dictionary<string, string>? SelectedOptions { get; set; }

        /// <summary>
        /// Gets or sets any additional notes regarding the cart item.
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Represents a request to update an existing cart item in the shopping cart.
    /// </summary>
    public class UpdateCartItemRequestDTO
    {
        /// <summary>
        /// Gets or sets the updated quantity of the cart item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the updated selected options for the cart item.
        /// </summary>
        public Dictionary<string, string>? SelectedOptions { get; set; }
    }

    /// <summary>
    /// Represents a request to apply a discount code to the shopping cart.
    /// </summary>
    public class ApplyDiscountRequestDTO
    {
        /// <summary>
        /// Gets or sets the discount code to be applied to the cart.
        /// </summary>
        public string DiscountCode { get; set; }
    }
}
