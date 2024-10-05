using System;

namespace ECommerceBackend.DTOs.Response.Order;

public class OrderItemDTO
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
