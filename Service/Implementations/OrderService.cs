using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Order;
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

            return OrderMapper.ToOrderResponseDTO(order);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(string orderId, string customerId)
        {
            // Retrieve the order from the database
            var order = await _context
                .Orders.Find(o => o.Id == orderId && o.CustomerId == customerId && !o.IsDeleted)
                .FirstOrDefaultAsync();

            // Return null if the order is not found or the customer is not authorized
            if (order == null)
            {
                return null;
            }

            // Map the order entity to the response DTO and return it
            return OrderMapper.ToOrderResponseDTO(order);
        }

        // Implementation for GetCustomerOrdersAsync
        public async Task<List<OrderResponseDTO>> GetCustomerOrdersAsync(string customerId)
        {
            // Retrieve all orders for the specified customer from the database
            var orders = await _context
                .Orders.Find(o => o.CustomerId == customerId && !o.IsDeleted)
                .ToListAsync();

            // Map the list of orders to a list of response DTOs and return it
            return orders.Select(OrderMapper.ToOrderResponseDTO).ToList();
        }
    }
}
