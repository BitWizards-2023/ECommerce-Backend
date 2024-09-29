using System;

namespace ECommerceBackend.DTOs.Request;

public class RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
