using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.DTOs.Response.Auth
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public AddressResponseDTO Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = string.Empty;
        public string Profile_pic { get; set; } = string.Empty;
    }
}
