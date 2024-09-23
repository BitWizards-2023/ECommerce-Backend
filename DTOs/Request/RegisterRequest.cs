public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AddressRequest Address { get; set; }
    public string PhoneNumber { get; set;} 
    public string Role { get; set;} 
}

public class AddressRequest
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}
