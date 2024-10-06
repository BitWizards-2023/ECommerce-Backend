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

        public OrderService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(
            string customerId,
            CreateOrderRequestDTO orderRequestDTO
        )
        {
            var orderItems = new List<OrderItem>();
            var products = new List<Product>(); // List to store products
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

                // Calculate total amount
                totalAmount += product.Price * itemDTO.Quantity;

                // Create an OrderItem and pass the VendorId
                var orderItem = OrderMapper.ToOrderItemModel(itemDTO, product.VendorId);
                orderItems.Add(orderItem);
                products.Add(product); // Add product to the list

                // Adjust stock levels
                await _context.Products.UpdateOneAsync(
                    p => p.Id == product.Id,
                    Builders<Product>.Update.Set(
                        p => p.StockLevel,
                        product.StockLevel - itemDTO.Quantity
                    )
                );
            }

            // Create the Order object using the mapped OrderItems and totalAmount
            var order = OrderMapper.ToOrderModel(
                orderRequestDTO,
                customerId,
                totalAmount,
                orderItems
            );

            // Save the order in the database
            await _context.Orders.InsertOneAsync(order);

            // Pass products instead of orderItems to the response DTO
            return OrderMapper.ToOrderResponseDTO(order, products);
        }

        public async Task<List<OrderResponseDTO>> GetOrdersAsync(
            string status,
            string customerId,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        )
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Eq(o => o.IsDeleted, false); // Exclude soft-deleted orders by default

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

            // Apply pagination
            var orders = await _context
                .Orders.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Retrieve all productIds from the orders
            var productIds = orders
                .SelectMany(o => o.Items.Select(i => i.ProductId))
                .Distinct()
                .ToList();

            // Fetch product details from the Product collection
            var products = await _context
                .Products.Find(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            // Map orders to DTOs including product details
            return orders.Select(order => OrderMapper.ToOrderResponseDTO(order, products)).ToList();
        }

        public async Task<List<OrderResponseDTO>> GetCustomerOrdersAsync(
            string customerId,
            string status,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        )
        {
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

            // Apply pagination
            var orders = await _context
                .Orders.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Fetch product details for all order items
            var productIds = orders
                .SelectMany(o => o.Items.Select(i => i.ProductId))
                .Distinct()
                .ToList();
            var products = await _context
                .Products.Find(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            // Map the orders to response DTOs
            return orders.Select(order => OrderMapper.ToOrderResponseDTO(order, products)).ToList();
        }

        public async Task<List<OrderResponseDTO>> GetVendorOrdersAsync(
            string vendorId,
            string status,
            DateTime? dateFrom,
            DateTime? dateTo,
            int pageNumber,
            int pageSize
        )
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter =
                filterBuilder.ElemMatch(o => o.Items, i => i.VendorId == vendorId)
                & filterBuilder.Eq(o => o.IsDeleted, false); // Exclude soft-deleted orders

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

            // Apply pagination
            var orders = await _context
                .Orders.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Fetch product details for all order items
            var productIds = orders
                .SelectMany(o => o.Items.Select(i => i.ProductId))
                .Distinct()
                .ToList();
            var products = await _context
                .Products.Find(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            // Filter items by vendor and map the orders to response DTOs
            return orders
                .Select(order => OrderMapper.ToVendorOrderResponseDTO(order, vendorId, products))
                .ToList();
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(
            string orderId,
            string userId,
            string userRole
        )
        {
            // Find the order by ID and ensure it's not soft-deleted
            var filter =
                Builders<Order>.Filter.Eq(o => o.Id, orderId)
                & Builders<Order>.Filter.Eq(o => o.IsDeleted, false);

            var order = await _context.Orders.Find(filter).FirstOrDefaultAsync();

            if (order == null)
            {
                return null;
            }

            // Check role-based access control
            if (userRole == "Administrator" || userRole == "CSR")
            {
                // Admins/CSRs have full access to all orders
                return OrderMapper.ToSingleOrderResponseDTO(order);
            }
            else if (userRole == "Customer")
            {
                // Customers can only access their own orders
                if (order.CustomerId != userId)
                {
                    return null; // Forbidden, customer trying to access another customer's order
                }
                return OrderMapper.ToSingleOrderResponseDTO(order);
            }
            else if (userRole == "Vendor")
            {
                // Vendors can only access orders containing their products
                var vendorOrderItems = order.Items.Where(i => i.VendorId == userId).ToList();
                if (!vendorOrderItems.Any())
                {
                    return null; // Forbidden, vendor has no products in this order
                }
                // Return the order details but only include the vendor's items
                return OrderMapper.ToVendorOrderResponseDTO(order, userId);
            }

            // If no valid role is found, return null
            return null;
        }

        public async Task<ResponseDTO<string>> UpdateOrderItemStatusAsync(
            string orderId,
            string itemId,
            string userId,
            string userRole,
            UpdateOrderItemStatusDTO updateOrderItemStatusDTO
        )
        {
            // Find the order by ID and ensure it's not soft-deleted
            var order = await _context
                .Orders.Find(o => o.Id == orderId && !o.IsDeleted)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ResponseDTO<string>(false, "Order not found", null);
            }

            // Find the specific item in the order
            var orderItem = order.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (orderItem == null)
            {
                return new ResponseDTO<string>(false, "Order item not found", null);
            }

            // Access control: ensure the vendor is only updating their own items
            if (userRole == "Vendor" && orderItem.VendorId != userId)
            {
                return new ResponseDTO<string>(
                    false,
                    "You are not authorized to update this item",
                    null
                );
            }

            // Vendors, Administrators, or CSR can update
            if (userRole != "Vendor" && userRole != "Administrator" && userRole != "CSR")
            {
                return new ResponseDTO<string>(false, "Unauthorized access", null);
            }

            // Validate the new status (add more validation logic if necessary)
            if (string.IsNullOrEmpty(updateOrderItemStatusDTO.Status))
            {
                return new ResponseDTO<string>(false, "Status is required", null);
            }

            // Update the order item status and tracking number (if provided)
            orderItem.Status = updateOrderItemStatusDTO.Status;
            if (!string.IsNullOrEmpty(updateOrderItemStatusDTO.TrackingNumber))
            {
                orderItem.TrackingNumber = updateOrderItemStatusDTO.TrackingNumber;
            }

            // Update the overall order status if all items are delivered or canceled
            if (order.Items.All(i => i.Status == "Delivered"))
            {
                order.Status = "Delivered";
            }
            else if (order.Items.All(i => i.Status == "Cancelled"))
            {
                order.Status = "Cancelled";
            }

            // Save changes to the order in the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == orderId, order);

            // Optionally, send notifications to the customer about the item status update

            return new ResponseDTO<string>(true, "Order item status updated successfully", null);
        }

        public async Task<ResponseDTO<string>> CancelOrderAsync(
            string orderId,
            string userId,
            string userRole,
            string reason = null
        )
        {
            // Find the order by ID and ensure it's not already canceled or soft-deleted
            var order = await _context
                .Orders.Find(o => o.Id == orderId && !o.IsDeleted && o.Status != "Cancelled")
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ResponseDTO<string>(false, "Order not found or already canceled", null);
            }

            // Check if the user is authorized to cancel the order
            if (userRole == "Customer" && order.CustomerId != userId)
            {
                return new ResponseDTO<string>(
                    false,
                    "You are not authorized to cancel this order",
                    null
                );
            }

            // Only vendors cannot cancel orders, Admins/CSRs can cancel any order
            if (userRole == "Vendor")
            {
                return new ResponseDTO<string>(
                    false,
                    "Vendors are not authorized to cancel orders",
                    null
                );
            }

            // Check if the order is in a cancellable state (not already shipped or delivered)
            if (order.Status == "Shipped" || order.Status == "Delivered")
            {
                return new ResponseDTO<string>(
                    false,
                    "Order cannot be canceled after shipment or delivery",
                    null
                );
            }

            // Soft-delete the order by marking it as deleted and updating the status to "Cancelled"
            order.IsDeleted = true;
            order.Status = "Cancelled";

            // Optionally restock the inventory for each item in the order
            foreach (var item in order.Items)
            {
                await _context.Products.UpdateOneAsync(
                    p => p.Id == item.ProductId,
                    Builders<Product>.Update.Inc(p => p.StockLevel, item.Quantity)
                );
            }

            // Optionally process a refund if the payment was captured
            if (order.PaymentStatus == "Completed")
            {
                // TODO: Integrate with payment gateway to process refunds
                order.PaymentStatus = "Refunded";
            }

            // Save the updated order to the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

            // Optionally send notifications to the customer and vendor about the cancellation
            // TODO: Send email or push notifications

            return new ResponseDTO<string>(true, "Order canceled successfully", null);
        }

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

            // Check if the order is in a state that can be confirmed as delivered
            if (order.Status != "Shipped" && order.Status != "Out for Delivery")
            {
                return new ResponseDTO<string>(
                    false,
                    "Order cannot be confirmed for delivery in its current state",
                    null
                );
            }

            // Update the order status to 'Delivered'
            order.Status = "Delivered";
            order.UpdatedAt = DateTime.UtcNow;

            // Optionally, update each item status to 'Delivered'
            foreach (var item in order.Items)
            {
                item.Status = "Delivered";
            }

            // Save the updated order to the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

            // Optionally send notifications to vendors and administrators
            // TODO: Send email or push notifications

            return new ResponseDTO<string>(true, "Order delivery confirmed successfully", null);
        }

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

            // Create the order note
            var orderNote = new OrderNote
            {
                Note = noteContent,
                AddedBy = userId,
                AddedAt = DateTime.UtcNow,
            };

            // Add the note to the internalNotes collection
            order.InternalNotes.Add(orderNote);

            // Update the order in the database
            await _context.Orders.ReplaceOneAsync(o => o.Id == order.Id, order);

            return new ResponseDTO<string>(true, "Note added successfully", null);
        }
    }
}
