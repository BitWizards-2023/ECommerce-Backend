using System;
using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Order;

namespace ECommerceBackend.Service.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDTO> CreateOrderAsync(
        string customerId,
        CreateOrderRequestDTO orderRequestDTO
    );
    Task<OrderResponseDTO> GetOrderByIdAsync(string orderId, string customerId);
    Task<List<OrderResponseDTO>> GetCustomerOrdersAsync(string customerId);
}
