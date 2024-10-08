/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the OrderService class,
 * which provides functionality for creating, retrieving, updating, and managing
 * orders in the ECommerceBackend application. It includes methods for order creation,
 * status updates, cancellations, and processing vendor and customer orders.
 * Date Created: 2024/09/18
 */

using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.DTOs.Response.Order;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Models;
using ECommerceBackend.Models.Entities;
using ECommerceBackend.Service.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceBackend.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly MongoDbContext _context;

        /// <summary>
        /// Constructor for OrderService, injecting the MongoDB context.
        /// </summary>
        /// <param name="context">The MongoDB context</param>
        public OrderService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a new order based on customer details and the products ordered.
        /// </summary>
        public async Task<OrderResponseDTO> CreateOrderAsync(
            string customerId,
            CreateOrderRequestDTO orderRequestDTO
        )
        {
            // Initialize variables for total amount and products list
            var orderItems = new List<OrderItem>();
            var products = new List<Product>();
            decimal totalAmount = 0;

            foreach (var itemDTO in orderRequestDTO.Items)
            {
                // Retrieve the product from the database
                var product = await _context
                    .Products.Find(p => p.Id == itemDTO.ProductId && p.IsActive)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    throw new InvalidOperationException(
                        $"Product with ID {itemDTO.ProductId} is not available."
                    );
                }

                // Validate stock levels
                if (product.StockLevel < itemDTO.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {product.Name}. Available: {product.StockLevel}, Requested: {itemDTO.Quantity}"
                    );
                }

                // Calculate the total order amount
                totalAmount += product.Price * itemDTO.Quantity;

                // Map order item and associate vendor ID
                var orderItem = OrderMapper.ToOrderItemModel(itemDTO, product.VendorId);
                orderItems.Add(orderItem);
                products.Add(product);

                // Adjust the stock levels in the database
                await _context.Products.UpdateOneAsync(
                    p => p.Id == product.Id,
                    Builders<Product>.Update.Set(
                        p => p.StockLevel,
                        product.StockLevel - itemDTO.Quantity
                    )
                );
            }

            // Create the order using the mapped order items and total amount
            var order = OrderMapper.ToOrderModel(
                orderRequestDTO,
                customerId,
                totalAmount,
                orderItems
            );

            // Save the order to the database
            await _context.Orders.InsertOneAsync(order);

            // Map the order to a response DTO and return the products list
            return OrderMapper.ToOrderResponseDTO(order, products);
        }

        /// <summary>
        /// Retrieves orders with optional filters for status, customer, date range, and pagination.
        /// </summary>
        public async Task<List<OrderResponseDTO>> GetOrdersAsync(
            string status,
            string customerId,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        )
        {
            // Build the query filter
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Eq(o => o.IsDeleted, false);

            // Apply filters based on the query parameters
            if (!string.IsNullOrEmpty(status))
            {
                filter = filter & filterBuilder.Eq(o => o.Status, status);
            }
            if (!string.IsNullOrEmpty(customerId))
            {
                filter = filter & filterBuilder.Eq(o => o.CustomerId, customerId);
            }
            if (dateFrom.HasValue)
            {
                filter = filter & filterBuilder.Gte(o => o.CreatedAt, dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                filter = filter & filterBuilder.Lte(o => o.CreatedAt, dateTo.Value);
            }

            // Fetch orders with pagination
            var orders = await _context
                .Orders.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Get product details for the orders
            var productIds = orders
                .SelectMany(o => o.Items.Select(i => i.ProductId))
                .Distinct()
                .ToList();
            var products = await _context
                .Products.Find(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            // Map the orders to response DTOs with product details
            return orders.Select(order => OrderMapper.ToOrderResponseDTO(order, products)).ToList();
        }

        /// <summary>
        /// Retrieves a customer's orders with optional filters and pagination.
        /// </summary>
        public async Task<List<OrderResponseDTO>> GetCustomerOrdersAsync(
            string customerId,
            string status,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        )
        {
            // Build the query filter for the customer's orders
            var filterBuilder = Builders<Order>.Filter;
            var filter =
                filterBuilder.Eq(o => o.CustomerId, customerId)
                & filterBuilder.Eq(o => o.IsDeleted, false);

            // Apply filters based on the query parameters
            if (!string.IsNullOrEmpty(status))
            {
                filter = filter & filterBuilder.Eq(o => o.Status, status);
            }
            if (dateFrom.HasValue)
            {
                filter = filter & filterBuilder.Gte(o => o.CreatedAt, dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                filter = filter & filterBuilder.Lte(o => o.CreatedAt, dateTo.Value);
            }

            // Fetch orders with pagination
            var orders = await _context
                .Orders.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Get product details for the orders
            var productIds = orders
                .SelectMany(o => o.Items.Select(i => i.ProductId))
                .Distinct()
                .ToList();
            var products = await _context
                .Products.Find(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            // Map the orders to response DTOs with product details
            return orders.Select(order => OrderMapper.ToOrderResponseDTO(order, products)).ToList();
        }

        /// <summary>
        /// Retrieves vendor orders with optional filters and pagination.
        /// </summary>
        public async Task<List<OrderResponseDTO>> GetVendorOrdersAsync(
            string vendorId,
            string status,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        )
        {
            // Build the query filter for the vendor's orders
            var filterBuilder = Builders<Order>.Filter;
            var filter =
                filterBuilder.ElemMatch(o => o.Items, i => i.VendorId == vendorId)
                & filterBuilder.Eq(o => o.IsDeleted, false);

            // Apply filters based on the query parameters
            if (!string.IsNullOrEmpty(status))
            {
                filter = filter & filterBuilder.Eq(o => o.Status, status);
            }
            if (dateFrom.HasValue)
            {
                filter = filter & filterBuilder.Gte(o => o.CreatedAt, dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                filter = filter & filterBuilder.Lte(o => o.CreatedAt, dateTo.Value);
            }

            // Fetch orders with pagination
            var orders = await _context
                .Orders.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Get product details for the orders
            var productIds = orders
                .SelectMany(o => o.Items.Select(i => i.ProductId))
                .Distinct()
                .ToList();
            var products = await _context
                .Products.Find(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            // Map the vendor orders to response DTOs with filtered product details
            return orders
                .Select(order => OrderMapper.ToVendorOrderResponseDTO(order, vendorId, products))
                .ToList();
        }

        /// <summary>
        /// Retrieves a specific order by its ID, ensuring role-based access control.
        /// </summary>
        public async Task<OrderResponseDTO> GetOrderByIdAsync(
            string orderId,
            string userId,
            string userRole
        )
        {
            // Build the filter to find the order by ID and ensure it's not soft-deleted
            var filter =
                Builders<Order>.Filter.Eq(o => o.Id, orderId)
                & Builders<Order>.Filter.Eq(o => o.IsDeleted, false);

            // Fetch the order from the database
            var order = await _context.Orders.Find(filter).FirstOrDefaultAsync();

            if (order == null)
            {
                return null; // Return null if the order doesn't exist
            }

            // Role-based access control
            if (userRole == "Administrator" || userRole == "CSR")
            {
                // Admins/CSRs have full access to all orders
                return OrderMapper.ToSingleOrderResponseDTO(order);
            }
            else if (userRole == "Customer" && order.CustomerId != userId)
            {
                // Customers can only access their own orders
                return null;
            }
            else if (userRole == "Vendor")
            {
                // Vendors can only access orders containing their products
                var vendorOrderItems = order.Items.Where(i => i.VendorId == userId).ToList();
                if (!vendorOrderItems.Any())
                {
                    return null; // Return null if the vendor has no products in this order
                }
                return OrderMapper.ToVendorOrderResponseDTO(order, userId);
            }

            return null; // No access if the role doesn't match any valid criteria
        }

        /// <summary>
        /// Updates the status of a specific item in an order (accessible to Vendor/Admin/CSR).
        /// </summary>
        public async Task<ResponseDTO<string>> UpdateOrderItemStatusAsync(
            string orderId,
            string itemId,
            string userId,
            string userRole,
            UpdateOrderItemStatusDTO updateOrderItemStatusDTO
        )
        {
            // Fetch the order by ID and ensure it's not soft-deleted
            var order = await _context
                .Orders.Find(o => o.Id == orderId && !o.IsDeleted)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ResponseDTO<string>(false, "Order not found", null);
            }

            // Find the specific order item by its ID
            var orderItem = order.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (orderItem == null)
            {
                return new ResponseDTO<string>(false, "Order item not found", null);
            }

            // Ensure that vendors can only update their own items
            if (userRole == "Vendor" && orderItem.VendorId != userId)
            {
                return new ResponseDTO<string>(false, "Unauthorized access to this item", null);
            }

            // Validate that a valid status has been provided
            if (string.IsNullOrEmpty(updateOrderItemStatusDTO.Status))
            {
                return new ResponseDTO<string>(false, "Status is required", null);
            }

            // Update the status and tracking number if provided
            orderItem.Status = updateOrderItemStatusDTO.Status;
            if (!string.IsNullOrEmpty(updateOrderItemStatusDTO.TrackingNumber))
            {
                orderItem.TrackingNumber = updateOrderItemStatusDTO.TrackingNumber;
            }

            // Update the overall order status based on individual item statuses
            if (order.Items.All(i => i.Status == "Delivered"))
            {
                order.Status = "Delivered";
            }
            else if (order.Items.All(i => i.Status == "Cancelled"))
            {
                order.Status = "Cancelled";
            }

            // Save the changes to the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == orderId, order);

            return new ResponseDTO<string>(true, "Order item status updated successfully", null);
        }

        /// <summary>
        /// Cancels an order, ensuring that the user has the appropriate permissions.
        /// </summary>
        public async Task<ResponseDTO<string>> CancelOrderAsync(
            string orderId,
            string userId,
            string userRole,
            string reason = null
        )
        {
            // Fetch the order by ID and ensure it's not already canceled or soft-deleted
            var order = await _context
                .Orders.Find(o => o.Id == orderId && !o.IsDeleted && o.Status != "Cancelled")
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ResponseDTO<string>(false, "Order not found or already canceled", null);
            }

            // Ensure that only the customer who placed the order can cancel it
            if (userRole == "Customer" && order.CustomerId != userId)
            {
                return new ResponseDTO<string>(false, "Unauthorized access to this order", null);
            }

            // Ensure that vendors cannot cancel orders
            if (userRole == "Vendor")
            {
                return new ResponseDTO<string>(
                    false,
                    "Vendors are not authorized to cancel orders",
                    null
                );
            }

            // Ensure that orders in certain states (e.g., shipped or delivered) cannot be canceled
            if (order.Status == "Shipped" || order.Status == "Delivered")
            {
                return new ResponseDTO<string>(
                    false,
                    "Order cannot be canceled after shipment or delivery",
                    null
                );
            }

            // Mark the order as canceled and soft-delete it
            order.IsDeleted = true;
            order.Status = "Cancelled";

            // Restock the inventory for each item in the order
            foreach (var item in order.Items)
            {
                await _context.Products.UpdateOneAsync(
                    p => p.Id == item.ProductId,
                    Builders<Product>.Update.Inc(p => p.StockLevel, item.Quantity)
                );
            }

            // Optionally process a refund if the payment has been completed
            if (order.PaymentStatus == "Completed")
            {
                // TODO: Integrate with payment gateway to process refunds
                order.PaymentStatus = "Refunded";
            }

            // Save the canceled order to the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

            return new ResponseDTO<string>(true, "Order canceled successfully", null);
        }

        /// <summary>
        /// Confirms the delivery of an order, updating its status to 'Delivered'.
        /// </summary>
        public async Task<ResponseDTO<string>> ConfirmOrderDeliveryAsync(
            string orderId,
            string customerId
        )
        {
            // Retrieve the order by ID and ensure it belongs to the customer
            var order = await _context
                .Orders.Find(o => o.Id == orderId && o.CustomerId == customerId && !o.IsDeleted)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ResponseDTO<string>(
                    false,
                    "Order not found or does not belong to the customer",
                    null
                );
            }

            // Check if the order is in a deliverable state
            if (order.Status != "Shipped" && order.Status != "Out for Delivery")
            {
                return new ResponseDTO<string>(
                    false,
                    "Order cannot be confirmed for delivery in its current state",
                    null
                );
            }

            // Mark the order and all its items as delivered
            order.Status = "Delivered";
            order.UpdatedAt = DateTime.UtcNow;
            foreach (var item in order.Items)
            {
                item.Status = "Delivered";
            }

            // Save the updated order to the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

            return new ResponseDTO<string>(true, "Order delivery confirmed successfully", null);
        }

        /// <summary>
        /// Adds an internal note to the order (Admins and CSRs only).
        /// </summary>
        public async Task<ResponseDTO<string>> AddNoteToOrderAsync(
            string orderId,
            string userId,
            string userRole,
            string noteContent
        )
        {
            // Retrieve the order by ID and ensure it exists
            var order = await _context
                .Orders.Find(o => o.Id == orderId && !o.IsDeleted)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ResponseDTO<string>(false, "Order not found", null);
            }

            // Add a note to the order's internal notes
            var orderNote = new OrderNote
            {
                Note = noteContent,
                AddedBy = userId,
                AddedAt = DateTime.UtcNow,
            };
            order.InternalNotes.Add(orderNote);

            // Save the updated order to the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

            return new ResponseDTO<string>(true, "Note added successfully", null);
        }
    }
}
