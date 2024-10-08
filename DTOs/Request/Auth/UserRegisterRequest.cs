using System;
using ECommerceBackend.DTOs.Request.Auth;

namespace ECommerceBackend.DTOs.Request.Auth;

public class UserRegisterRequest
{
    public string Password { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public AddressRequest Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = string.Empty;
    public string Profile_pic { get; set; } = string.Empty;
    public string FcmTokens { get; set; } = string.Empty;
}
