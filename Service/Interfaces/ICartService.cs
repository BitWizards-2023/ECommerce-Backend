/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the ICartService interface, which provides
 * functionality for managing shopping cart operations in an e-commerce system.
 * It includes methods for retrieving, adding, updating, removing items from the cart,
 * applying discounts, and checking out.
 * Date Created: 2024/09/28
 */

using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Cart;
using ECommerceBackend.DTOs.Response.Cart;

namespace ECommerceBackend.Service.Interfaces
{
    public interface ICartService
    {
        /// <summary>
        /// Retrieves the shopping cart for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A CartResponseDTO containing the user's cart details</returns>
        Task<CartResponseDTO> GetCartAsync(string userId);

        /// <summary>
        /// Adds or updates an item in the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="cartItemRequestDTO">The item details to be added or updated in the cart</param>
        /// <returns>A CartResponseDTO containing the updated cart details</returns>
        Task<CartResponseDTO> AddOrUpdateCartItemAsync(
            string userId,
            CartItemRequestDTO cartItemRequestDTO
        );

        /// <summary>
        /// Updates a specific cart item.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="cartItemId">The ID of the cart item to be updated</param>
        /// <param name="updateCartItemRequestDTO">The new cart item details</param>
        /// <returns>A CartResponseDTO containing the updated cart details</returns>
        Task<CartResponseDTO> UpdateCartItemAsync(
            string userId,
            string cartItemId,
            UpdateCartItemRequestDTO updateCartItemRequestDTO
        );

        /// <summary>
        /// Removes an item from the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="cartItemId">The ID of the cart item to be removed</param>
        /// <returns>True if the item was removed successfully, false otherwise</returns>
        Task<bool> RemoveCartItemAsync(string userId, string cartItemId);

        /// <summary>
        /// Clears the entire shopping cart for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>True if the cart was cleared successfully, false otherwise</returns>
        Task<bool> ClearCartAsync(string userId);

        /// <summary>
        /// Applies a discount code to the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="applyDiscountRequestDTO">The discount code to be applied</param>
        /// <returns>A CartResponseDTO containing the updated cart details with the applied discount</returns>
        Task<CartResponseDTO> ApplyDiscountAsync(
            string userId,
            ApplyDiscountRequestDTO applyDiscountRequestDTO
        );

        /// <summary>
        /// Proceeds to checkout for the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A CartResponseDTO containing the finalized cart details after checkout</returns>
        Task<CartResponseDTO> CheckoutAsync(string userId);
    }
}
