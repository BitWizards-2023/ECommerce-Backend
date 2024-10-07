namespace ECommerceBackend.DTOs.Response.Vendor
{
    public class VendorProfileResponseDTO
    {
        public string VendorId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressDTO Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePic { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<VendorRatingResponseDTO> Ratings { get; set; } =
            new List<VendorRatingResponseDTO>();
    }

    public class AddressDTO
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class VendorRatingResponseDTO
    {
        public string CustomerId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
