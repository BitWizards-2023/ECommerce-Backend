namespace ECommerceBackend.DTOs.Request.Auth;

public class RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
