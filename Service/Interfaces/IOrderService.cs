/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the IOrderService interface, which provides functionality
 * for managing customer and vendor orders in the e-commerce system. It includes methods for
 * creating, updating, retrieving, and canceling orders, as well as updating order item statuses,
 * confirming deliveries, and adding notes to orders.
 * Date Created: 2024/09/28
 */

using System;
using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.DTOs.Response.Order;

namespace ECommerceBackend.Service.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer placing the order</param>
        /// <param name="orderRequestDTO">The order details</param>
        /// <returns>An OrderResponseDTO containing the created order details</returns>
        Task<OrderResponseDTO> CreateOrderAsync(
            string customerId,
            CreateOrderRequestDTO orderRequestDTO
        );

        /// <summary>
        /// Retrieves a list of orders with optional filters for status, customer, date range, and pagination.
        /// </summary>
        /// <param name="status">The status of the orders to filter</param>
        /// <param name="customerId">The ID of the customer (optional)</param>
        /// <param name="dateFrom">The start date to filter orders (optional)</param>
        /// <param name="dateTo">The end date to filter orders (optional)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <param name="pageSize">The page size for pagination</param>
        /// <returns>A list of OrderResponseDTO containing the orders that match the filter criteria</returns>
        Task<List<OrderResponseDTO>> GetOrdersAsync(
            string status,
            string customerId,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        );

        /// <summary>
        /// Retrieves a list of customer-specific orders with optional filters and pagination.
        /// </summary>
        /// <param name="customerId">The ID of the customer</param>
        /// <param name="status">The status of the orders to filter (optional)</param>
        /// <param name="dateFrom">The start date to filter orders (optional)</param>
        /// <param name="dateTo">The end date to filter orders (optional)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <param name="pageSize">The page size for pagination</param>
        /// <returns>A list of OrderResponseDTO containing the customer's orders</returns>
        Task<List<OrderResponseDTO>> GetCustomerOrdersAsync(
            string customerId,
            string status,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        );

        /// <summary>
        /// Retrieves a list of vendor-specific orders with optional filters and pagination.
        /// </summary>
        /// <param name="vendorId">The ID of the vendor</param>
        /// <param name="status">The status of the orders to filter (optional)</param>
        /// <param name="dateFrom">The start date to filter orders (optional)</param>
        /// <param name="dateTo">The end date to filter orders (optional)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <param name="pageSize">The page size for pagination</param>
        /// <returns>A list of OrderResponseDTO containing the vendor's orders</returns>
        Task<List<OrderResponseDTO>> GetVendorOrdersAsync(
            string vendorId,
            string status,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        );

        /// <summary>
        /// Retrieves a specific order by its ID, with role-based access control.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <param name="userId">The ID of the user retrieving the order</param>
        /// <param name="userRole">The role of the user (e.g., Admin, Vendor, Customer)</param>
        /// <returns>An OrderResponseDTO containing the order details</returns>
        Task<OrderResponseDTO> GetOrderByIdAsync(string orderId, string userId, string userRole);

        /// <summary>
        /// Updates the status of a specific item in an order.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <param name="itemId">The ID of the item to update</param>
        /// <param name="userId">The ID of the user updating the item</param>
        /// <param name="userRole">The role of the user (e.g., Admin, Vendor, Customer)</param>
        /// <param name="updateOrderItemStatusDTO">The updated status and tracking details</param>
        /// <returns>A ResponseDTO indicating the result of the update</returns>
        Task<ResponseDTO<string>> UpdateOrderItemStatusAsync(
            string orderId,
            string itemId,
            string userId,
            string userRole,
            UpdateOrderItemStatusDTO updateOrderItemStatusDTO
        );

        /// <summary>
        /// Cancels an order, ensuring role-based access control.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel</param>
        /// <param name="userId">The ID of the user canceling the order</param>
        /// <param name="userRole">The role of the user (e.g., Admin, Customer)</param>
        /// <param name="reason">The reason for cancellation (optional)</param>
        /// <returns>A ResponseDTO indicating the result of the cancellation</returns>
        Task<ResponseDTO<string>> CancelOrderAsync(
            string orderId,
            string userId,
            string userRole,
            string reason = null
        );

        /// <summary>
        /// Confirms the delivery of an order by the customer.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <param name="customerId">The ID of the customer confirming the delivery</param>
        /// <returns>A ResponseDTO indicating the result of the delivery confirmation</returns>
        Task<ResponseDTO<string>> ConfirmOrderDeliveryAsync(string orderId, string customerId);

        /// <summary>
        /// Adds an internal note to the order, accessible by Admins and CSRs.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <param name="userId">The ID of the user adding the note</param>
        /// <param name="userRole">The role of the user (e.g., Admin, CSR)</param>
        /// <param name="noteContent">The content of the note</param>
        /// <returns>A ResponseDTO indicating the result of adding the note</returns>
        Task<ResponseDTO<string>> AddNoteToOrderAsync(
            string orderId,
            string userId,
            string userRole,
            string noteContent
        );
    }
}
