using System.Collections.Generic;
using System.Linq;
using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Order;
using ECommerceBackend.Models.Entities;
using OrderItemDTO = ECommerceBackend.DTOs.Response.Order.OrderItemDTO;

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

        public static OrderResponseDTO ToOrderResponseDTO(Order order, List<Product> products)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = new DTOs.Response.Order.AddressDTO
                {
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country,
                },
                PaymentStatus = order.PaymentStatus,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(item => ToOrderItemDTO(item, products)).ToList(),
            };
        }

        // Maps OrderItem to OrderItemDTO including product details
        public static OrderItemDTO ToOrderItemDTO(OrderItem orderItem, List<Product> products)
        {
            // Get product details for the order item
            var product = products.FirstOrDefault(p => p.Id == orderItem.ProductId);

            return new OrderItemDTO
            {
                ProductId = orderItem.ProductId,
                VendorId = orderItem.VendorId,
                Quantity = orderItem.Quantity,
                Status = orderItem.Status,
                ProductDetails =
                    product != null
                        ? new ProductDetailsDTO
                        {
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Images = product.Images,
                        }
                        : null // Return null if the product is not found
                ,
            };
        }

        public static OrderResponseDTO ToVendorOrderResponseDTO(
            Order order,
            string vendorId,
            List<Product> products
        )
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = new DTOs.Response.Order.AddressDTO
                {
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country,
                },
                PaymentStatus = order.PaymentStatus,
                CreatedAt = order.CreatedAt,
                Items = order
                    .Items.Where(item => item.VendorId == vendorId) // Only include items for this vendor
                    .Select(item => ToOrderItemDTO(item, products))
                    .ToList(),
            };
        }

        private static string GenerateOrderNumber()
        {
            return $"O{DateTime.UtcNow.Ticks}";
        }

        public static OrderResponseDTO ToVendorOrderResponseDTO(Order order, string vendorId)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = new DTOs.Response.Order.AddressDTO
                {
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country,
                },
                PaymentStatus = order.PaymentStatus,
                CreatedAt = order.CreatedAt,
                // Filter the items to only include the vendor's products
                Items = order
                    .Items.Where(item => item.VendorId == vendorId)
                    .Select(ToOrderItemDTO)
                    .ToList(),
            };
        }

        public static OrderItemDTO ToOrderItemDTO(OrderItem orderItem)
        {
            return new OrderItemDTO
            {
                ProductId = orderItem.ProductId,
                VendorId = orderItem.VendorId,
                Quantity = orderItem.Quantity,
                Status = orderItem.Status,
            };
        }

        public static OrderResponseDTO ToSingleOrderResponseDTO(Order order)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = new DTOs.Response.Order.AddressDTO
                {
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country,
                },
                PaymentStatus = order.PaymentStatus,
                CreatedAt = order.CreatedAt,
                Items = order
                    .Items.Select(item => new OrderItemDTO
                    {
                        ProductId = item.ProductId,
                        VendorId = item.VendorId,
                        Quantity = item.Quantity,
                        Status = item.Status,
                    })
                    .ToList(),
            };
        }
    }
}
