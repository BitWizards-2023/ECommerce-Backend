using ECommerceBackend.DTOs.Request.Auth;
using ECommerceBackend.DTOs.Response.Auth;

namespace ECommerceBackend.Services
{
    public interface IAuthService
    {
        string Authenticate(string email, string password, out string refreshToken);

        bool Register(RegisterRequest registerRequest);

        string RefreshToken(string token, string refreshToken);

        void Logout(string email);

        string RequestPasswordReset(string email);

        bool ResetPassword(string email, string resetToken, string newPassword);

        UserResponseDTO GetCurrentUser(string userId);
    }
}
