/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the CartResponseDTO and CartItemResponseDTO, which are used to return
 * details about the shopping cart and its items in response to cart-related API requests. It includes
 * properties for cart items, total amount, discounts, shipping estimates, and checkout status.
 * Date Created: 2024/09/28
 */

using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Cart
{
    /// <summary>
    /// Represents the details of a shopping cart returned in response to cart-related API requests.
    /// </summary>
    public class CartResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the list of items in the cart.
        /// </summary>
        public List<CartItemResponseDTO> Items { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the cart before discounts and shipping.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount code applied to the cart (if any).
        /// </summary>
        public string? DiscountCode { get; set; }

        /// <summary>
        /// Gets or sets the amount of discount applied to the cart.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the estimated shipping cost.
        /// </summary>
        public decimal EstimatedShipping { get; set; }

        /// <summary>
        /// Gets or sets the estimated tax amount.
        /// </summary>
        public decimal EstimatedTax { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cart has been checked out.
        /// </summary>
        public bool IsCheckedOut { get; set; }

        /// <summary>
        /// Gets or sets additional notes related to the cart (optional).
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Represents the details of an individual item in the shopping cart.
    /// </summary>
    public class CartItemResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart item.
        /// </summary>
        public string CartItemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the product in the cart item.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vendor who sells the product.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the cart item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the selected options for the product (e.g., size, color).
        /// </summary>
        public Dictionary<string, string>? SelectedOptions { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the total price of the cart item (quantity * price).
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the status of the cart item (e.g., "In Stock", "Out of Stock").
        /// </summary>
        public string Status { get; set; }
    }
}
