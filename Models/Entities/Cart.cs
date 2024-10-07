using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceBackend.Models.Entities
{
    public class Cart
    {
        // Unique identifier for the cart
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        // Reference to the authenticated user who owns the cart
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } // Required, as carts are only for authenticated users

        // List of items in the cart
        [BsonElement("items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Date and time when the cart was last updated
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Applied discount or coupon codes
        [BsonElement("discountCode")]
        public string? DiscountCode { get; set; }

        // Total discount amount
        [BsonElement("discountAmount")]
        public decimal DiscountAmount { get; set; } = 0.0m;

        // Estimated shipping cost
        [BsonElement("estimatedShipping")]
        public decimal EstimatedShipping { get; set; } = 0.0m;

        // Estimated tax amount
        [BsonElement("estimatedTax")]
        public decimal EstimatedTax { get; set; } = 0.0m;

        // Flag indicating if the cart has been checked out
        [BsonElement("isCheckedOut")]
        public bool IsCheckedOut { get; set; } = false;

        // Additional notes or instructions for the entire cart
        [BsonElement("notes")]
        public string? Notes { get; set; }

        // Date and time when the cart was created
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CartItem
    {
        // Unique identifier for the cart item
        [BsonElement("cartItemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CartItemId { get; set; } = ObjectId.GenerateNewId().ToString();

        // Reference to the product
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = string.Empty;

        // Reference to the vendor supplying the product
        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } = string.Empty;

        // Quantity of the product
        [BsonElement("quantity")]
        public int Quantity { get; set; } = 1;

        // Selected options (e.g., size, color)
        [BsonElement("selectedOptions")]
        public Dictionary<string, string>? SelectedOptions { get; set; }

        // Price per unit at the time of adding to cart
        [BsonElement("price")]
        public decimal Price { get; set; } = 0.0m;

        // Total price for this item (price * quantity)
        [BsonElement("totalPrice")]
        public decimal TotalPrice
        {
            get { return Price * Quantity; }
        }

        // Status of the cart item (e.g., "Pending")
        [BsonElement("status")]
        public string Status { get; set; } = "Pending";

        // Date and time when the item was added to the cart
        [BsonElement("addedAt")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Optional: Notes or special instructions for the item
        [BsonElement("notes")]
        public string? Notes { get; set; }
    }
}
