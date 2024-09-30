using ECommerceBackend.DTOs.Response;
using ECommerceBackend.Models;

namespace ECommerceBackend.Services
{
    public interface IAuthService
    {
        string Authenticate(string email, string password, out string refreshToken);

        bool Register(
            string email,
            string password,
            string username,
            string role,
            string firstName,
            string lastName,
            AddressRequest address,
            string phoneNumber,
            string ProfilePic
        );

        string RefreshToken(string token, string refreshToken);

        void Logout(string email);

        string RequestPasswordReset(string email);

        bool ResetPassword(string email, string resetToken, string newPassword);

        UserResponseDTO GetCurrentUser(string userId);
    }
}
