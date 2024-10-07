using ECommerceBackend.DTOs.Request.SubmitRatings;
using ECommerceBackend.DTOs.Response.Vendor;

namespace ECommerceBackend.Services.Interfaces
{
    public interface IVendorRatingService
    {
        Task<bool> SubmitRatingAsync(
            string vendorId,
            SubmitRatingRequestDTO ratingDTO,
            string customerId
        );
        Task<bool> ApproveRatingAsync(string ratingId);
        Task<VendorProfileResponseDTO> GetVendorProfileAsync(string vendorId);
    }
}
