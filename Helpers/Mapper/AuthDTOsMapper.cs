/*
 * File: AuthDTOsMapper.cs
 * Description: Provides mapping methods to convert authentication-related DTOs to User models.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-09-29
 */

using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.Models;
using ECommerceBackend.Utilities;

namespace ECommerceBackend.Helpers.Mapper
{
    public class AuthDTOsMapper
    {
        /// <summary>
        /// Converts a RegisterRequest DTO to a User model.
        /// </summary>
        /// <param name="registerRequest">The registration request containing user details.</param>
        /// <returns>A User model populated with the provided registration details.</returns>
        // Maps the RegisterRequest DTO to a User model, hashing the password and setting default values.
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
                    ? "https://shopilystorage.blob.core.windows.net/mycontainer/cd4e13a7-7ce1-4c63-beb4-4e44cdfa10f0.png"
                    : registerRequest.Profile_pic,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsActive = registerRequest.IsActive,

                FcmToken = string.IsNullOrWhiteSpace(registerRequest.FcmToken)
                    ? string.Empty
                    : registerRequest.FcmToken,
            };
        }

        /// <summary>
        /// Maps the UserUpdateRequest DTO to an existing User model.
        /// </summary>
        /// <param name="user">The existing User entity.</param>
        /// <param name="updateRequest">The update request DTO with new values.</param>
        public static void UpdateUserModel(User user, UserUpdateRequest updateRequest)
        {
            user.FirstName = updateRequest.FirstName;
            user.LastName = updateRequest.LastName;
            user.Email = updateRequest.Email;
            user.PhoneNumber = updateRequest.PhoneNumber;
            user.Address.Street = updateRequest.Address.Street;
            user.Address.City = updateRequest.Address.City;
            user.Address.State = updateRequest.Address.State;
            user.Address.PostalCode = updateRequest.Address.PostalCode;
            user.Address.Country = updateRequest.Address.Country;
            user.UpdatedAt = DateTime.UtcNow;
        }
    }
}
