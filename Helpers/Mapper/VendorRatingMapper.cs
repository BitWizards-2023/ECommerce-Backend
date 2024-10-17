using System.Collections.Generic;
using System.Linq;
using ECommerceBackend.DTOs.Response.Vendor;
using ECommerceBackend.Models;
using ECommerceBackend.Models.Entities;

namespace ECommerceBackend.Helpers.Mapper
{
    public static class VendorMapper
    {
        public static VendorProfileResponseDTO ToVendorProfileResponseDTO(
            User vendor,
            double averageRating,
            int totalReviews,
            List<VendorRating> vendorRatings
        )
        {
            return new VendorProfileResponseDTO
            {
                VendorId = vendor.Id,
                UserName = vendor.Username,
                Email = vendor.Email,
                FirstName = vendor.FirstName,
                LastName = vendor.LastName,
                Address = new AddressDTO
                {
                    Street = vendor.Address.Street,
                    City = vendor.Address.City,
                    State = vendor.Address.State,
                    PostalCode = vendor.Address.PostalCode,
                    Country = vendor.Address.Country,
                },
                PhoneNumber = vendor.PhoneNumber,
                ProfilePic = vendor.ProfilePic,
                AverageRating = averageRating,
                TotalReviews = totalReviews,
                Ratings = vendorRatings.Select(ToVendorRatingResponseDTO).ToList(),
            };
        }

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
