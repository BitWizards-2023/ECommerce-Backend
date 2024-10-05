using System;

namespace ECommerceBackend.DTOs.Request.Order;

public class OrderItemDTO
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
