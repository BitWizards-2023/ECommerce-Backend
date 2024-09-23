using System;

namespace ECommerceBackend.DTOs.Request;

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string ResetToken { get; set; }
    public string NewPassword { get; set; }
}
