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
                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { id = order.Id },
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

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order details.</returns>
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized(new ResponseDTO<string>(false, "Invalid customer ID", null));
            }

            try
            {
                // Assuming the service fetches the order and checks if the customer is authorized to view the order
                var order = await _orderService.GetOrderByIdAsync(id, customerId);

                if (order == null)
                {
                    return NotFound(new ResponseDTO<string>(false, "Order not found", null));
                }

                return Ok(
                    new ResponseDTO<OrderResponseDTO>(true, "Order retrieved successfully", order)
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
        /// Retrieves all orders for the authenticated customer.
        /// </summary>
        /// <returns>A list of the customer's orders.</returns>
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerOrders()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized(new ResponseDTO<string>(false, "Invalid customer ID", null));
            }

            try
            {
                var orders = await _orderService.GetCustomerOrdersAsync(customerId);

                if (orders == null || orders.Count == 0)
                {
                    return NotFound(
                        new ResponseDTO<string>(false, "No orders found for the customer", null)
                    );
                }

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
    }
}
