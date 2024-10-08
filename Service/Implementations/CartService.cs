/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the CartService class,
 * which provides functionality for managing the shopping cart in the
 * ECommerceBackend application. It includes methods for adding, updating,
 * and removing items in the cart, applying discounts, and proceeding to checkout.
 * Date Created: 2024/09/18
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Cart;
using ECommerceBackend.DTOs.Response.Cart;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Models.Entities;
using ECommerceBackend.Service.Interfaces;
using MongoDB.Driver;

namespace ECommerceBackend.Service.Implementations
{
    public class CartService : ICartService
    {
        private readonly MongoDbContext _context;

        /// <summary>
        /// Constructor for CartService, injecting MongoDB context.
        /// </summary>
        /// <param name="context">The MongoDB context</param>
        public CartService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieve the current user's cart. If no cart exists, creates a new one.
        /// </summary>
        public async Task<CartResponseDTO> GetCartAsync(string userId)
        {
            // Fetch the user's cart or create a new one if it doesn't exist
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _context.Carts.InsertOneAsync(cart);
            }

            return CartMapper.ToCartResponseDTO(cart);
        }

        /// <summary>
        /// Add or update items in the cart with stock validation.
        /// </summary>
        public async Task<CartResponseDTO> AddOrUpdateCartItemAsync(
            string userId,
            CartItemRequestDTO cartItemRequestDTO
        )
        {
            // Fetch the user's cart or create a new one if it doesn't exist
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
            }

            // Validate the product and its stock
            var product = await _context
                .Products.Find(p => p.Id == cartItemRequestDTO.ProductId && p.IsActive)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                throw new InvalidOperationException(
                    $"Product with ID {cartItemRequestDTO.ProductId} does not exist or is inactive."
                );
            }

            // Validate stock levels
            if (product.StockLevel < cartItemRequestDTO.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for product {product.Name}. Available: {product.StockLevel}, Requested: {cartItemRequestDTO.Quantity}"
                );
            }

            // Add or update the cart item
            var existingItem = cart.Items.FirstOrDefault(i =>
                i.ProductId == cartItemRequestDTO.ProductId
            );
            if (existingItem != null)
            {
                existingItem.Quantity += cartItemRequestDTO.Quantity;
                existingItem.Price = product.Price;
                existingItem.SelectedOptions = cartItemRequestDTO.SelectedOptions;
            }
            else
            {
                var cartItem = CartMapper.ToCartItemModel(cartItemRequestDTO, product);
                cart.Items.Add(cartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);

            return CartMapper.ToCartResponseDTO(cart);
        }

        /// <summary>
        /// Update a specific cart item's quantity or options.
        /// </summary>
        public async Task<CartResponseDTO> UpdateCartItemAsync(
            string userId,
            string cartItemId,
            UpdateCartItemRequestDTO updateCartItemRequestDTO
        )
        {
            // Fetch the user's cart
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            // Validate the cart item
            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException($"Cart item with ID {cartItemId} not found.");
            }

            // Validate stock levels for the updated quantity
            var product = await _context
                .Products.Find(p => p.Id == cartItem.ProductId)
                .FirstOrDefaultAsync();
            if (product.StockLevel < updateCartItemRequestDTO.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for product {product.Name}. Available: {product.StockLevel}, Requested: {updateCartItemRequestDTO.Quantity}"
                );
            }

            // Update the cart item details
            cartItem.Quantity = updateCartItemRequestDTO.Quantity;
            cartItem.SelectedOptions = updateCartItemRequestDTO.SelectedOptions;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return CartMapper.ToCartResponseDTO(cart);
        }

        /// <summary>
        /// Remove an item from the cart.
        /// </summary>
        public async Task<bool> RemoveCartItemAsync(string userId, string cartItemId)
        {
            // Fetch the user's cart
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            // Validate the cart item
            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException($"Cart item with ID {cartItemId} not found.");
            }

            // Remove the item from the cart
            cart.Items.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return true;
        }

        /// <summary>
        /// Clear the entire cart.
        /// </summary>
        public async Task<bool> ClearCartAsync(string userId)
        {
            // Fetch the user's cart
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            // Clear the cart items
            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return true;
        }

        /// <summary>
        /// Apply a discount to the cart.
        /// </summary>
        public async Task<CartResponseDTO> ApplyDiscountAsync(
            string userId,
            ApplyDiscountRequestDTO applyDiscountRequestDTO
        )
        {
            // Fetch the user's cart
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            // Apply the discount code
            if (applyDiscountRequestDTO.DiscountCode == "DISCOUNT10")
            {
                cart.DiscountCode = applyDiscountRequestDTO.DiscountCode;
                cart.DiscountAmount = cart.Items.Sum(i => i.TotalPrice) * 0.1m;
            }
            else
            {
                throw new InvalidOperationException("Invalid discount code.");
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return CartMapper.ToCartResponseDTO(cart);
        }

        /// <summary>
        /// Proceed to checkout for the current user's cart.
        /// </summary>
        public async Task<CartResponseDTO> CheckoutAsync(string userId)
        {
            // Fetch the user's cart
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            if (!cart.Items.Any())
            {
                throw new InvalidOperationException(
                    "Cannot proceed to checkout with an empty cart."
                );
            }

            // Mark the cart as checked out
            cart.IsCheckedOut = true;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return CartMapper.ToCartResponseDTO(cart);
        }
    }
}
