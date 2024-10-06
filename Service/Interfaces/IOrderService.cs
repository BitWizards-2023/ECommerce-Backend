using System;
using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.DTOs.Response.Order;

namespace ECommerceBackend.Service.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDTO> CreateOrderAsync(
        string customerId,
        CreateOrderRequestDTO orderRequestDTO
    );

    Task<List<OrderResponseDTO>> GetOrdersAsync(
        string status,
        string customerId,
        DateTime? dateFrom,
        DateTime? dateTo,
        int pageNumber,
        int pageSize
    );

    Task<List<OrderResponseDTO>> GetCustomerOrdersAsync(
        string customerId,
        string status,
        DateTime? dateFrom,
        DateTime? dateTo,
        int pageNumber,
        int pageSize
    );

    Task<List<OrderResponseDTO>> GetVendorOrdersAsync(
        string vendorId,
        string status,
        DateTime? dateFrom,
        DateTime? dateTo,
        int pageNumber,
        int pageSize
    );
    Task<OrderResponseDTO> GetOrderByIdAsync(string orderId, string userId, string userRole);

    Task<ResponseDTO<string>> UpdateOrderItemStatusAsync(
        string orderId,
        string itemId,
        string userId,
        string userRole,
        UpdateOrderItemStatusDTO updateOrderItemStatusDTO
    );
    Task<ResponseDTO<string>> CancelOrderAsync(
        string orderId,
        string userId,
        string userRole,
        string reason = null
    );
    Task<ResponseDTO<string>> ConfirmOrderDeliveryAsync(string orderId, string customerId);
    Task<ResponseDTO<string>> AddNoteToOrderAsync(
        string orderId,
        string userId,
        string userRole,
        string noteContent
    );
}
