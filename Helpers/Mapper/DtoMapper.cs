/*
 * File: DtoMapper.cs
 * Description: Provides mapping methods to convert User models to various response DTOs.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-09-29
 */

using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.Models;

namespace ECommerceBackend.Helpers
{
    public static class DtoMapper
    {
        /// <summary>
        /// Converts a User model to a UserResponseDTO.
        /// </summary>
        /// <param name="user">The User model to be converted.</param>
        /// <returns>A UserResponseDTO populated with the user's details.</returns>
        // Maps the User model to a UserResponseDTO, including nested Address details if available.
        public static UserResponseDTO ToUserResponseDTO(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Profile_pic = user.ProfilePic,
                Address =
                    user.Address != null
                        ? new AddressResponseDTO
                        {
                            Street = user.Address.Street,
                            City = user.Address.City,
                            State = user.Address.State,
                            PostalCode = user.Address.PostalCode,
                            Country = user.Address.Country,
                        }
                        : null,
            };
        }
    }
}
