using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.Data.Repository.Interfaces;

public interface IUserServices
{
    List<UserResponseDTO> GetUserList();
}
