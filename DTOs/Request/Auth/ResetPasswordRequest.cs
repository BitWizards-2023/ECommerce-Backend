namespace ECommerceBackend.DTOs.Request.Auth;

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string ResetToken { get; set; }
    public string NewPassword { get; set; }
}
