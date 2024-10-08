/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the CartController class,
 * which provides the functionality for managing the shopping cart in the ECommerceBackend
 * application. It includes methods to retrieve, add, update, and remove items from the cart,
 * as well as applying discounts and checking out.
 * Date Created: 2024/09/18
 */

using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Cart;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/v1/cart")]
    [ApiController]
    [Authorize(Policy = "CustomerPolicy")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Constructor for CartController, injecting the CartService.
        /// </summary>
        /// <param name="cartService">Service for managing cart operations</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Retrieves the current user's cart.
        /// </summary>
        /// <returns>The cart object for the current user.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Retrieve cart for the current user
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        /// <summary>
        /// Adds or updates an item in the user's cart.
        /// </summary>
        /// <param name="cartItemRequest">The item to add or update in the cart.</param>
        /// <returns>The updated cart.</returns>
        [HttpPut("items")]
        public async Task<IActionResult> AddOrUpdateItem(
            [FromBody] CartItemRequestDTO cartItemRequest
        )
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Add or update the cart item for the user
            var cart = await _cartService.AddOrUpdateCartItemAsync(userId, cartItemRequest);
            return Ok(cart);
        }

        /// <summary>
        /// Updates the quantity or details of an existing cart item.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to update.</param>
        /// <param name="updateCartItemRequest">The updated item details.</param>
        /// <returns>The updated cart.</returns>
        [HttpPatch("items/{cartItemId}")]
        public async Task<IActionResult> UpdateItem(
            string cartItemId,
            [FromBody] UpdateCartItemRequestDTO updateCartItemRequest
        )
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Update the cart item for the user
            var cart = await _cartService.UpdateCartItemAsync(
                userId,
                cartItemId,
                updateCartItemRequest
            );
            return Ok(cart);
        }

        /// <summary>
        /// Removes an item from the user's cart.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        /// <returns>The result of the removal operation.</returns>
        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(string cartItemId)
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Remove the specified cart item for the user
            var result = await _cartService.RemoveCartItemAsync(userId, cartItemId);
            return Ok(result);
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <returns>The result of the clear cart operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Clear the user's cart
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Applies a discount code to the user's cart.
        /// </summary>
        /// <param name="discountRequest">The discount code to apply.</param>
        /// <returns>The updated cart after applying the discount.</returns>
        [HttpPost("apply-discount")]
        public async Task<IActionResult> ApplyDiscount(
            [FromBody] ApplyDiscountRequestDTO discountRequest
        )
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Apply the discount to the user's cart
            var cart = await _cartService.ApplyDiscountAsync(userId, discountRequest);
            return Ok(cart);
        }

        /// <summary>
        /// Proceeds to checkout the user's cart.
        /// </summary>
        /// <returns>The cart after the checkout operation.</returns>
        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Checkout the user's cart
            var cart = await _cartService.CheckoutAsync(userId);
            return Ok(cart);
        }
    }
}
