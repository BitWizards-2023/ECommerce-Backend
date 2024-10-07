using ECommerceBackend.DTOs.Response.Vendor;
using ECommerceBackend.Models.Entities; // Use the correct namespace for VendorRating

namespace ECommerceBackend.Helpers.Mapper
{
    public static class VendorRatingMapper
    {
        public static VendorRatingResponseDTO ToVendorRatingResponseDTO(VendorRating vendorRating)
        {
            return new VendorRatingResponseDTO
            {
                CustomerId = vendorRating.CustomerId,
                Comment = vendorRating.Comment,
                Rating = vendorRating.Rating,
                CreatedAt = vendorRating.CreatedAt,
            };
        }
    }
}
