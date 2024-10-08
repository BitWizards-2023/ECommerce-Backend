using System;

namespace ECommerceBackend.DTOs.Response.Order;

public class OrderItemDTO
{
    public string ProductId { get; set; }
    public string VendorId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
    public ProductDetailsDTO ProductDetails { get; set; }
}
