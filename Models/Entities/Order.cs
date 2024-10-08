using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceBackend.Models.Entities
{
    public class Order
    {
        // Unique identifier for the order
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        // Human-readable order number (e.g., "O1001")
        [BsonElement("orderNumber")]
        public string OrderNumber { get; set; } = string.Empty;

        // Reference to the customer who placed the order
        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = string.Empty;

        // List of items in the order
        [BsonElement("items")]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Total amount of the order
        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; } = 0.0m;

        // Overall status of the order (e.g., "Processing", "Shipped", "Delivered", "Cancelled")
        [BsonElement("status")]
        public string Status { get; set; } = "Processing";

        // Shipping address for the order
        [BsonElement("shippingAddress")]
        public Address ShippingAddress { get; set; } = new Address();

        // Payment method used for the order
        [BsonElement("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;

        // Payment status (e.g., "Pending", "Completed", "Failed")
        [BsonElement("paymentStatus")]
        public string PaymentStatus { get; set; } = "Pending";

        // Flag indicating whether the order has been soft deleted
        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        // Date and time when the order was created
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Date and time when the order was last updated
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Additional notes or comments about the order
        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;

        // List of internal notes added by administrators or CSRs
        [BsonElement("internalNotes")]
        public List<OrderNote> InternalNotes { get; set; } = new List<OrderNote>();

        // List of notifications related to the order
        [BsonElement("notifications")]
        public List<OrderNotification> Notifications { get; set; } = new List<OrderNotification>();
    }

    public class OrderItem
    {
        // Unique identifier for the order item
        [BsonElement("itemId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ItemId { get; set; } = ObjectId.GenerateNewId().ToString();

        // Reference to the product
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = string.Empty;

        // Reference to the vendor supplying the product
        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } = string.Empty;

        // Quantity of the product ordered
        [BsonElement("quantity")]
        public int Quantity { get; set; } = 1;

        // Price per unit at the time of purchase
        [BsonElement("price")]
        public decimal Price { get; set; } = 0.0m;

        // Status of the individual item (e.g., "Processing", "Shipped", "Delivered", "Cancelled")
        [BsonElement("status")]
        public string Status { get; set; } = "Processing";

        // Tracking number for the shipment of this item
        [BsonElement("trackingNumber")]
        public string TrackingNumber { get; set; } = string.Empty;

        // Any notes specific to this item
        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
    }

    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; } = string.Empty;

        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("state")]
        public string State { get; set; } = string.Empty;

        [BsonElement("postalCode")]
        public string PostalCode { get; set; } = string.Empty;

        [BsonElement("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class OrderNote
    {
        // Note content
        [BsonElement("note")]
        public string Note { get; set; } = string.Empty;

        // User ID of the person who added the note
        [BsonElement("addedBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AddedBy { get; set; } = string.Empty;

        // Date and time when the note was added
        [BsonElement("addedAt")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderNotification
    {
        // Notification message
        [BsonElement("message")]
        public string Message { get; set; } = string.Empty;

        // Date and time when the notification was created
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Flag indicating whether the notification has been read
        [BsonElement("isRead")]
        public bool IsRead { get; set; } = false;
    }
}
