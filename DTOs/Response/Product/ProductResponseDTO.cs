using System;

namespace ECommerceBackend.DTOs.Response.Product;

public class ProductResponseDTO
{
    public string Id { get; set; }
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> CategoryIds { get; set; }
    public string VendorId { get; set; }
    public List<string> Images { get; set; }
    public List<ProductAttributeResponseDTO> Attributes { get; set; }
    public int StockLevel { get; set; }
    public int LowStockThreshold { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public double AverageRating { get; set; }
    public int RatingsCount { get; set; }
}

public class ProductAttributeResponseDTO
{
    public string Name { get; set; }
    public string Value { get; set; }
}
