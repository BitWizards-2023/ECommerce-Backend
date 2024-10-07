using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Cart;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/v1/cart")]
    [ApiController]
    [Authorize(Policy = "CustomerPolicy")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPut("items")]
        public async Task<IActionResult> AddOrUpdateItem(
            [FromBody] CartItemRequestDTO cartItemRequest
        )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.AddOrUpdateCartItemAsync(userId, cartItemRequest);
            return Ok(cart);
        }

        [HttpPatch("items/{cartItemId}")]
        public async Task<IActionResult> UpdateItem(
            string cartItemId,
            [FromBody] UpdateCartItemRequestDTO updateCartItemRequest
        )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.UpdateCartItemAsync(
                userId,
                cartItemId,
                updateCartItemRequest
            );
            return Ok(cart);
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(string cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.RemoveCartItemAsync(userId, cartItemId);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }

        [HttpPost("apply-discount")]
        public async Task<IActionResult> ApplyDiscount(
            [FromBody] ApplyDiscountRequestDTO discountRequest
        )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.ApplyDiscountAsync(userId, discountRequest);
            return Ok(cart);
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.CheckoutAsync(userId);
            return Ok(cart);
        }
    }
}
