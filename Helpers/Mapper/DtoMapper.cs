using ECommerceBackend.DTOs.Response;
using ECommerceBackend.Models;

namespace ECommerceBackend.Helpers
{
    public static class DtoMapper
    {
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
