using System;

namespace ECommerceBackend.DTOs.Request.Product;

public class ProductRequestDTO
{
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> CategoryIds { get; set; }
    public List<string> Images { get; set; }
    public List<ProductAttributeDTO> Attributes { get; set; }
    public int StockLevel { get; set; }
    public int LowStockThreshold { get; set; }
    public bool IsFeatured { get; set; }
}

public class ProductAttributeDTO
{
    public string Name { get; set; }
    public string Value { get; set; }
}
