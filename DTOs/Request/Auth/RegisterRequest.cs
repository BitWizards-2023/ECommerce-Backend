namespace ECommerceBackend.DTOs.Request.Auth;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public AddressRequest Address { get; set; } = new AddressRequest();
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer";
    public string Profile_pic { get; set; } = string.Empty;
    public string FcmToken { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
}
