using System;

namespace ECommerceBackend.DTOs.Request.Order;

public class UpdateOrderItemStatusDTO
{
    public string Status { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; } // Optional
}
