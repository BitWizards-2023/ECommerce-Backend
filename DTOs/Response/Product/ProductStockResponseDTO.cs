using System;

namespace ECommerceBackend.DTOs.Response.Product;

public class ProductStockResponseDTO
{
    public string ProductId { get; set; }
    public int StockLevel { get; set; }
}
