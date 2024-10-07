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

        public CartService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Retrieve the current user's cart
        public async Task<CartResponseDTO> GetCartAsync(string userId)
        {
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

        // Add or update items in the cart
        public async Task<CartResponseDTO> AddOrUpdateCartItemAsync(
            string userId,
            CartItemRequestDTO cartItemRequestDTO
        )
        {
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

            var product = await _context
                .Products.Find(p => p.Id == cartItemRequestDTO.ProductId && p.IsActive)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                throw new InvalidOperationException(
                    $"Product with ID {cartItemRequestDTO.ProductId} does not exist or is inactive."
                );
            }

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

        // Update a specific cart item's quantity or options
        public async Task<CartResponseDTO> UpdateCartItemAsync(
            string userId,
            string cartItemId,
            UpdateCartItemRequestDTO updateCartItemRequestDTO
        )
        {
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException($"Cart item with ID {cartItemId} not found.");
            }

            cartItem.Quantity = updateCartItemRequestDTO.Quantity;
            cartItem.SelectedOptions = updateCartItemRequestDTO.SelectedOptions;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return CartMapper.ToCartResponseDTO(cart);
        }

        // Remove an item from the cart
        public async Task<bool> RemoveCartItemAsync(string userId, string cartItemId)
        {
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException($"Cart item with ID {cartItemId} not found.");
            }

            cart.Items.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return true;
        }

        // Clear the entire cart
        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return true;
        }

        // Apply a discount to the cart
        public async Task<CartResponseDTO> ApplyDiscountAsync(
            string userId,
            ApplyDiscountRequestDTO applyDiscountRequestDTO
        )
        {
            var cart = await _context
                .Carts.Find(c => c.UserId == userId && !c.IsCheckedOut)
                .FirstOrDefaultAsync();
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            // For simplicity, assume a flat 10% discount for valid codes
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

        // Proceed to checkout
        public async Task<CartResponseDTO> CheckoutAsync(string userId)
        {
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

            cart.IsCheckedOut = true;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return CartMapper.ToCartResponseDTO(cart);
        }
    }
}
