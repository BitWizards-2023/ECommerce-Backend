using System;
using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.DTOs.Request.Auth;

public class UserUpdateRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public AddressRequest Address { get; set; }
}
