using System;
using ECommerceBackend.DTOs.Request;
using ECommerceBackend.Models;
using ECommerceBackend.Utilities;

namespace ECommerceBackend.Helpers.Mapper;

public class AuthDTOsMapper
{
    public static User ToUserModel(RegisterRequest registerRequest)
    {
        return new User
        {
            Email = registerRequest.Email,
            Username = registerRequest.Username,
            PasswordHash = PasswordHasher.HashPassword(registerRequest.Password),
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Address = new Address
            {
                Street = registerRequest.Address.Street,
                City = registerRequest.Address.City,
                State = registerRequest.Address.State,
                PostalCode = registerRequest.Address.PostalCode,
                Country = registerRequest.Address.Country,
                IsDeleted = false,
            },
            PhoneNumber = registerRequest.PhoneNumber,
            Role = string.IsNullOrWhiteSpace(registerRequest.Role)
                ? "Customer"
                : registerRequest.Role,
            ProfilePic = string.IsNullOrWhiteSpace(registerRequest.Profile_pic)
                ? "https://default_profile_pic.png"
                : registerRequest.Profile_pic,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false,
        };
    }
}
