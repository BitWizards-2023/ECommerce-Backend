using System;

namespace ECommerceBackend.DTOs.Response.Order;

public class AddressDTO
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}
