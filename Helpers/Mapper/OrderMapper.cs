using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Order;
using ECommerceBackend.Models.Entities;
using AddressDTO = ECommerceBackend.DTOs.Response.Order.AddressDTO;

namespace ECommerceBackend.Helpers.Mapper
{
    public static class OrderMapper
    {
        public static Order ToOrderModel(
            CreateOrderRequestDTO orderRequestDTO,
            string customerId,
            decimal totalAmount,
            List<OrderItem> orderItems
        )
        {
            return new Order
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerId = customerId,
                Items = orderItems, // Pass the pre-constructed order items with vendorId
                TotalAmount = totalAmount,
                ShippingAddress = new Address
                {
                    Street = orderRequestDTO.ShippingAddress.Street,
                    City = orderRequestDTO.ShippingAddress.City,
                    State = orderRequestDTO.ShippingAddress.State,
                    PostalCode = orderRequestDTO.ShippingAddress.PostalCode,
                    Country = orderRequestDTO.ShippingAddress.Country,
                },
                PaymentMethod = orderRequestDTO.PaymentMethod,
                Status = "Processing",
                PaymentStatus = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        // Add vendorId as a parameter
        public static OrderItem ToOrderItemModel(
            DTOs.Request.Order.OrderItemDTO orderItemDTO,
            string vendorId
        )
        {
            return new OrderItem
            {
                ProductId = orderItemDTO.ProductId,
                VendorId = vendorId, // Assign vendorId from the Product
                Quantity = orderItemDTO.Quantity,
                Status = "Processing",
            };
        }

        public static OrderResponseDTO ToOrderResponseDTO(Order order)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = new AddressDTO
                {
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country,
                },
                PaymentStatus = order.PaymentStatus,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(ToOrderItemDTO).ToList(),
            };
        }

        public static DTOs.Response.Order.OrderItemDTO ToOrderItemDTO(OrderItem orderItem)
        {
            return new DTOs.Response.Order.OrderItemDTO
            {
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
            };
        }

        private static string GenerateOrderNumber()
        {
            return $"O{DateTime.UtcNow.Ticks}";
        }
    }
}
