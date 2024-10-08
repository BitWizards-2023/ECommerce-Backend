using System;

namespace ECommerceBackend.DTOs.Response.Auth;

public class AddressResponseDTO
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}
