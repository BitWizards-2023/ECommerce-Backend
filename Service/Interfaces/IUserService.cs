using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.Data.Repository.Interfaces;

public interface IUserServices
{
    List<UserResponseDTO> GetUserList();
    UserResponseDTO GetUserById(string id);
    UserResponseDTO CreateUser(UserRegisterRequest userRegisterRequest);
    bool UpdateUser(string id, UserUpdateRequest userUpdateDTO);
    bool DeleteUser(string id);
    bool ActivateUser(string id);
    bool ApproveUser(string id);

    bool UpdateFcmToken(string userId, string fcmToken);
}
