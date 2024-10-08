/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the VendorRatingService class,
 * which provides functionality for submitting, approving, and retrieving vendor ratings.
 * It includes methods for submitting a rating, approving ratings, and getting a vendor's profile
 * with rating details.
 * Date Created: 2024/09/28
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.SubmitRatings; // Correct namespace reference for SubmitRatingRequestDTO
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

        /// <summary>
        /// Constructor for VendorRatingService, injecting the MongoDB context.
        /// </summary>
        /// <param name="context">The MongoDB context</param>
        public VendorRatingService(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Submits a rating and comment for a specific vendor.
        /// Ensures that the customer has made a purchase from the vendor before allowing rating submission.
        /// </summary>
        /// <param name="vendorId">The ID of the vendor</param>
        /// <param name="ratingDTO">The rating and comment details</param>
        /// <param name="customerId">The ID of the customer submitting the rating</param>
        /// <returns>True if the rating was submitted successfully</returns>
        public async Task<bool> SubmitRatingAsync(
            string vendorId,
            SubmitRatingRequestDTO ratingDTO, // Ensure correct type for ratingDTO
            string customerId
        )
        {
            // Check if the vendor exists in the Users collection with a role of "Vendor"
            var vendor = await _context
                .Users.Find(u => u.Id == vendorId && u.Role == "Vendor")
                .FirstOrDefaultAsync();
            if (vendor == null)
            {
                throw new InvalidOperationException($"Vendor with ID {vendorId} not found.");
            }

            // Verify if the customer has completed an order with this vendor
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

            // Create a new VendorRating entity and store it in the VendorRatings collection
            var rating = new VendorRating
            {
                VendorId = vendorId,
                CustomerId = customerId,
                Rating = ratingDTO.Rating,
                Comment = ratingDTO.Comment,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false, // Rating needs approval before being visible
            };

            // Insert the rating into the database
            await _context.VendorRatings.InsertOneAsync(rating);
            return true;
        }

        /// <summary>
        /// Approves a vendor rating, making it visible to others.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to approve</param>
        /// <returns>True if the rating was approved successfully</returns>
        public async Task<bool> ApproveRatingAsync(string ratingId)
        {
            // Update the IsApproved flag to true for the specified rating
            var update = Builders<VendorRating>.Update.Set(r => r.IsApproved, true);
            var result = await _context.VendorRatings.UpdateOneAsync(r => r.Id == ratingId, update);
            return result.ModifiedCount > 0; // Return true if the rating was approved
        }

        /// <summary>
        /// Retrieves the profile of a vendor, including their ratings and comments.
        /// </summary>
        /// <param name="vendorId">The ID of the vendor</param>
        /// <returns>A VendorProfileResponseDTO containing the vendor's details and ratings</returns>
        public async Task<VendorProfileResponseDTO> GetVendorProfileAsync(string vendorId)
        {
            // Retrieve vendor details from the Users collection
            var vendor = await _context
                .Users.Find(u => u.Id == vendorId && u.Role == "Vendor")
                .FirstOrDefaultAsync();
            if (vendor == null)
            {
                throw new InvalidOperationException($"Vendor with ID {vendorId} not found.");
            }

            // Retrieve all approved ratings and comments for the vendor
            var ratings = await _context
                .VendorRatings.Find(r => r.VendorId == vendorId && r.IsApproved)
                .ToListAsync();
            var averageRating = ratings.Any() ? ratings.Average(r => r.Rating) : 0;
            var totalReviews = ratings.Count;

            // Build and return the VendorProfileResponseDTO with vendor details and ratings
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
                Ratings = ratings
                    .Select(r => VendorRatingMapper.ToVendorRatingResponseDTO(r))
                    .ToList(), // Map ratings to VendorRatingResponseDTO
            };
        }
    }
}
