using System.Collections.Generic;
using ECommerceBackend.DTOs.Request.Cart;
using ECommerceBackend.DTOs.Response.Cart;
using ECommerceBackend.DTOs.Response.Product;
using ECommerceBackend.Models.Entities;

namespace ECommerceBackend.Helpers.Mapper
{
    public static class CartMapper
    {
        public static CartItem ToCartItemModel(CartItemRequestDTO requestDTO, Product product)
        {
            return new CartItem
            {
                ProductId = requestDTO.ProductId,
                VendorId = product.VendorId,
                Quantity = requestDTO.Quantity,
                SelectedOptions = requestDTO.SelectedOptions,
                Price = product.Price,
                Status = "Pending",
                Notes = requestDTO.Notes,
                AddedAt = DateTime.UtcNow,
            };
        }

        public static CartResponseDTO ToCartResponseDTO(Cart cart)
        {
            return new CartResponseDTO
            {
                Id = cart.Id,
                Items = cart.Items.ConvertAll(ToCartItemResponseDTO),
                TotalAmount = cart.Items.Sum(i => i.TotalPrice),
                DiscountCode = cart.DiscountCode,
                DiscountAmount = cart.DiscountAmount,
                EstimatedShipping = cart.EstimatedShipping,
                EstimatedTax = cart.EstimatedTax,
                IsCheckedOut = cart.IsCheckedOut,
                Notes = cart.Notes,
            };
        }

        public static CartItemResponseDTO ToCartItemResponseDTO(CartItem item)
        {
            return new CartItemResponseDTO
            {
                CartItemId = item.CartItemId,
                ProductId = item.ProductId,
                VendorId = item.VendorId,
                Quantity = item.Quantity,
                SelectedOptions = item.SelectedOptions,
                Price = item.Price,
                TotalPrice = item.TotalPrice,
                Status = item.Status,
            };
        }
    }
}
