namespace ECommerceBackend.Services
{
    public interface IAuthService
    {
        // Authenticates a user and provides both JWT and refresh token
        string Authenticate(string email, string password, out string refreshToken);

        // Registers a new user
        bool Register(string email, string password);

        // Generates a new JWT using a valid refresh token
        string RefreshToken(string token, string refreshToken);

        // Logs out a user by invalidating their refresh token
        void Logout(string email);

         // Requests a password reset
        string RequestPasswordReset(string email);

        // Resets the password using the provided reset token
        bool ResetPassword(string email, string resetToken, string newPassword);
    }
}
