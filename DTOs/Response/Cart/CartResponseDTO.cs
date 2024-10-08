using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Cart
{
    public class CartResponseDTO
    {
        public string Id { get; set; }
        public List<CartItemResponseDTO> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal EstimatedShipping { get; set; }
        public decimal EstimatedTax { get; set; }
        public bool IsCheckedOut { get; set; }
        public string? Notes { get; set; }
    }

    public class CartItemResponseDTO
    {
        public string CartItemId { get; set; }
        public string ProductId { get; set; }
        public string VendorId { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, string>? SelectedOptions { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
