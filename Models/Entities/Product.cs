using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceBackend.Models.Entities;

public class Product
{
    // Unique identifier for the product
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Unique product code or SKU (Stock Keeping Unit)
    [BsonElement("productCode")]
    public string ProductCode { get; set; } = string.Empty;

    // Name of the product
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    // Detailed description of the product
    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    // Current price of the product
    [BsonElement("price")]
    public decimal Price { get; set; } = 0.0m;

    // List of category IDs the product belongs to
    [BsonElement("categoryIds")]
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> CategoryIds { get; set; } = new List<string>();

    // ID of the vendor offering the product
    [BsonElement("vendorId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string VendorId { get; set; } = string.Empty;

    // List of image URLs or file paths
    [BsonElement("images")]
    public List<string> Images { get; set; } = new List<string>();

    // Stock level of the product
    [BsonElement("stockLevel")]
    public int StockLevel { get; set; } = 0;

    // Low stock threshold for alerting purposes
    [BsonElement("lowStockThreshold")]
    public int LowStockThreshold { get; set; } = 10;

    // Flag indicating if the product is active and available for purchase
    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    // Flag indicating if the product is featured
    [BsonElement("isFeatured")]
    public bool IsFeatured { get; set; } = false;

    // Date and time when the product was created
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Date and time when the product was last updated
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Additional attributes or specifications
    [BsonElement("attributes")]
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();

    // Average customer rating
    [BsonElement("averageRating")]
    public double AverageRating { get; set; } = 0.0;

    // Number of ratings received
    [BsonElement("ratingsCount")]
    public int RatingsCount { get; set; } = 0;
}

// Model for product attributes (e.g., color, size)
public class ProductAttribute
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("value")]
    public string Value { get; set; } = string.Empty;
}
