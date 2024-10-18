using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.SubmitRatings; // Make sure you refer to the correct namespace here
using ECommerceBackend.DTOs.Response.Vendor;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Models;
using ECommerceBackend.Models.Entities;
using ECommerceBackend.Services.Interfaces;
using MongoDB.Driver;

namespace ECommerceBackend.Services.Implementations
{
    public class VendorRatingService : IVendorRatingService
    {
        private readonly MongoDbContext _context;

        public VendorRatingService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> SubmitRatingAsync(
            string vendorId,
            SubmitRatingRequestDTO ratingDTO, // Ensure the type is correct here as well
            string customerId
        )
        {
            // Verify that the vendor exists (handled by User model)
            var vendor = await _context
                .Users.Find(u => u.Id == vendorId && u.Role == "Vendor")
                .FirstOrDefaultAsync();
            if (vendor == null)
            {
                throw new InvalidOperationException($"Vendor with ID {vendorId} not found.");
            }

            // Check if the customer has completed an order with this vendor
            var orderExists = await _context
                .Orders.Find(o =>
                    o.CustomerId == customerId && o.Items.Any(i => i.VendorId == vendorId)
                )
                .AnyAsync();
            if (!orderExists)
            {
                throw new InvalidOperationException(
                    "You can only rate vendors you have purchased from."
                );
            }

            // Create and store the vendor rating
            var rating = new VendorRating
            {
                VendorId = vendorId,
                CustomerId = customerId,
                Rating = ratingDTO.Rating,
                Comment = ratingDTO.Comment,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false,
            };

            await _context.VendorRatings.InsertOneAsync(rating);
            return true;
        }

        public async Task<bool> ApproveRatingAsync(string ratingId)
        {
            var update = Builders<VendorRating>.Update.Set(r => r.IsApproved, true);
            var result = await _context.VendorRatings.UpdateOneAsync(r => r.Id == ratingId, update);
            return result.ModifiedCount > 0;
        }

        public async Task<VendorProfileResponseDTO> GetVendorProfileAsync(string vendorId)
        {
            var vendor = await _context
                .Users.Find(u => u.Id == vendorId && u.Role == "Vendor")
                .FirstOrDefaultAsync();
            if (vendor == null)
            {
                throw new InvalidOperationException($"Vendor with ID {vendorId} not found.");
            }

            var ratings = await _context
                .VendorRatings.Find(r => r.VendorId == vendorId && r.IsApproved)
                .ToListAsync();
            var averageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
            var totalReviews = ratings.Count;

            return VendorMapper.ToVendorProfileResponseDTO(
                vendor,
                averageRating,
                totalReviews,
                ratings
            );
        }

        public async Task<List<VendorProfileResponseDTO>> GetVendorsAsync()
        {
            var vendors = await _context.Users.Find(u => u.Role == "Vendor").ToListAsync();
            var vendorList = new List<VendorProfileResponseDTO>();

            foreach (var vendor in vendors)
            {
                var ratings = await _context
                    .VendorRatings.Find(r => r.VendorId == vendor.Id && r.IsApproved)
                    .ToListAsync();
                var averageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
                var totalReviews = ratings.Count;

                vendorList.Add(
                    VendorMapper.ToVendorProfileResponseDTO(
                        vendor,
                        averageRating,
                        totalReviews,
                        ratings
                    )
                );
            }

            return vendorList;
        }

        public async Task<List<VendorProfileResponseDTO>> GetAllVendorsAsync()
        {
            var vendors = await _context.Users.Find(u => u.Role == "Vendor").ToListAsync();
            var vendorList = new List<VendorProfileResponseDTO>();

            foreach (var vendor in vendors)
            {
                var ratings = await _context
                    .VendorRatings.Find(r => r.VendorId == vendor.Id)
                    .ToListAsync();
                var averageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
                var totalReviews = ratings.Count;

                vendorList.Add(
                    VendorMapper.ToVendorProfileResponseDTO(
                        vendor,
                        averageRating,
                        totalReviews,
                        ratings
                    )
                );
            }

            return vendorList;
        }
    }
}
