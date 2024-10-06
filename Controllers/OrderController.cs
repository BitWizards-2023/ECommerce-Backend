using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Order;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.DTOs.Response.Order;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderRequestDTO">The details of the order being created.</param>
        /// <returns>The created order details.</returns>
        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderRequestDTO orderRequestDTO
        )
        {
            // Get the customer ID from the authenticated user's claims
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized(new ResponseDTO<string>(false, "Invalid customer ID", null));
            }

            try
            {
                // Create the order via the order service
                var order = await _orderService.CreateOrderAsync(customerId, orderRequestDTO);

                // Return a 201 Created response with the order details
                return Ok(
                    new ResponseDTO<OrderResponseDTO>(true, "Order created successfully", order)
                );
            }
            catch (InvalidOperationException ex)
            {
                // Return a bad request if there's an issue with product availability or stock
                return BadRequest(new ResponseDTO<string>(false, ex.Message, null));
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for other exceptions
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        [Authorize(Policy = "AdminOrCSRPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string status = null,
            [FromQuery] string customerId = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(
                        new ResponseDTO<string>(false, "Invalid pagination parameters", null)
                    );
                }

                // Call the service to retrieve orders with optional filters and pagination
                var orders = await _orderService.GetOrdersAsync(
                    status,
                    customerId,
                    dateFrom,
                    dateTo,
                    pageNumber,
                    pageSize
                );

                return Ok(
                    new ResponseDTO<List<OrderResponseDTO>>(
                        true,
                        "Orders retrieved successfully",
                        orders
                    )
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        /// <summary>
        /// Retrieves all orders for the current authenticated customer with optional filters and pagination.
        /// </summary>
        /// <param name="status">Optional filter for order status.</param>
        /// <param name="dateFrom">Optional filter for orders created after this date.</param>
        /// <param name="dateTo">Optional filter for orders created before this date.</param>
        /// <param name="pageNumber">Optional page number for pagination (default is 1).</param>
        /// <param name="pageSize">Optional page size for pagination (default is 10).</param>
        /// <returns>A list of orders matching the filter criteria for the current customer.</returns>
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomerOrders(
            [FromQuery] string status = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            try
            {
                // Extract the customer ID from the authentication token
                var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(customerId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(
                            false,
                            "Invalid token or customer ID not found",
                            null
                        )
                    );
                }

                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(
                        new ResponseDTO<string>(false, "Invalid pagination parameters", null)
                    );
                }

                // Retrieve customer orders with optional filters and pagination
                var orders = await _orderService.GetCustomerOrdersAsync(
                    customerId,
                    status,
                    dateFrom,
                    dateTo,
                    pageNumber,
                    pageSize
                );

                return Ok(
                    new ResponseDTO<List<OrderResponseDTO>>(
                        true,
                        "Orders retrieved successfully",
                        orders
                    )
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        /// <summary>
        /// Retrieves all orders containing the vendor's products with optional filters and pagination.
        /// </summary>
        /// <param name="status">Optional filter for order status.</param>
        /// <param name="dateFrom">Optional filter for orders created after this date.</param>
        /// <param name="dateTo">Optional filter for orders created before this date.</param>
        /// <param name="pageNumber">Optional page number for pagination (default is 1).</param>
        /// <param name="pageSize">Optional page size for pagination (default is 10).</param>
        /// <returns>A list of orders containing the vendor's products.</returns>
        [Authorize(Policy = "VendorPolicy")]
        [HttpGet("vendor")]
        public async Task<IActionResult> GetVendorOrders(
            [FromQuery] string status = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            try
            {
                // Extract the vendor ID from the authentication token
                var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(vendorId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(false, "Invalid token or vendor ID not found", null)
                    );
                }

                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(
                        new ResponseDTO<string>(false, "Invalid pagination parameters", null)
                    );
                }

                // Retrieve vendor orders with optional filters and pagination
                var orders = await _orderService.GetVendorOrdersAsync(
                    vendorId,
                    status,
                    dateFrom,
                    dateTo,
                    pageNumber,
                    pageSize
                );

                return Ok(
                    new ResponseDTO<List<OrderResponseDTO>>(
                        true,
                        "Orders retrieved successfully",
                        orders
                    )
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        /// <summary>
        /// Updates the status of a specific item in an order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="itemId">The ID of the item to update.</param>
        /// <param name="updateOrderItemStatusDTO">The new status details.</param>
        /// <returns>A response indicating success or failure.</returns>
        [Authorize]
        [HttpPut("{orderId}/items/{itemId}")]
        public async Task<IActionResult> UpdateOrderItemStatus(
            string orderId,
            string itemId,
            [FromBody] UpdateOrderItemStatusDTO updateOrderItemStatusDTO
        )
        {
            try
            {
                // Get the user's role and ID
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(false, "Invalid token or user role", null)
                    );
                }

                // Attempt to update the item status
                var result = await _orderService.UpdateOrderItemStatusAsync(
                    orderId,
                    itemId,
                    userId,
                    userRole,
                    updateOrderItemStatusDTO
                );

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>A response indicating success or failure.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(
            string id,
            [FromBody] CancelOrderDTO cancelOrderDTO = null
        )
        {
            try
            {
                // Get the user's role and ID
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(false, "Invalid token or user role", null)
                    );
                }

                // Attempt to cancel the order
                var result = await _orderService.CancelOrderAsync(
                    id,
                    userId,
                    userRole,
                    cancelOrderDTO?.Reason
                );

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        /// <summary>
        /// Confirms the delivery of an order by the customer.
        /// </summary>
        /// <param name="id">The ID of the order to confirm delivery for.</param>
        /// <returns>A response indicating success or failure.</returns>
        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost("{id}/confirm-delivery")]
        public async Task<IActionResult> ConfirmOrderDelivery(string id)
        {
            try
            {
                // Get the authenticated customer's ID
                var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(customerId))
                {
                    return Unauthorized(
                        new ResponseDTO<string>(false, "Invalid token or user ID", null)
                    );
                }

                // Confirm the order delivery
                var result = await _orderService.ConfirmOrderDeliveryAsync(id, customerId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }

        /// <summary>
        /// Adds a note to the order (Administrator and CSR only).
        /// </summary>
        /// <param name="id">The ID of the order to add a note to.</param>
        /// <param name="addNoteDTO">The note content.</param>
        /// <returns>A response indicating success or failure.</returns>
        [Authorize(Policy = "AdminOrCSRPolicy")]
        [HttpPost("{id}/notes")]
        public async Task<IActionResult> AddNoteToOrder(string id, [FromBody] AddNoteDTO addNoteDTO)
        {
            try
            {
                // Validate note content
                if (string.IsNullOrWhiteSpace(addNoteDTO.Note))
                {
                    return BadRequest(
                        new ResponseDTO<string>(false, "Note content cannot be empty", null)
                    );
                }

                // Get the ID of the user adding the note
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                // Add the note to the order
                var result = await _orderService.AddNoteToOrderAsync(
                    id,
                    userId,
                    userRole,
                    addNoteDTO.Note
                );

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ResponseDTO<string>(false, $"An error occurred: {ex.Message}", null)
                );
            }
        }
    }
}
