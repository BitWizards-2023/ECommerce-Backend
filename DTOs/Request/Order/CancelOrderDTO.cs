using System;

namespace ECommerceBackend.DTOs.Request.Order;

public class CancelOrderDTO
{
    public string Reason { get; set; } = string.Empty;
}
