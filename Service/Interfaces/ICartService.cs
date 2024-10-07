using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Cart;
using ECommerceBackend.DTOs.Response.Cart;

namespace ECommerceBackend.Service.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDTO> GetCartAsync(string userId);
        Task<CartResponseDTO> AddOrUpdateCartItemAsync(
            string userId,
            CartItemRequestDTO cartItemRequestDTO
        );
        Task<CartResponseDTO> UpdateCartItemAsync(
            string userId,
            string cartItemId,
            UpdateCartItemRequestDTO updateCartItemRequestDTO
        );
        Task<bool> RemoveCartItemAsync(string userId, string cartItemId);
        Task<bool> ClearCartAsync(string userId);
        Task<CartResponseDTO> ApplyDiscountAsync(
            string userId,
            ApplyDiscountRequestDTO applyDiscountRequestDTO
        );
        Task<CartResponseDTO> CheckoutAsync(string userId);
    }
}
