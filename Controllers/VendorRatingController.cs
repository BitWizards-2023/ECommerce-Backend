using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.SubmitRatings; // Correct namespace
using ECommerceBackend.DTOs.Response.Vendor;
using ECommerceBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorRatingController : ControllerBase
    {
        private readonly IVendorRatingService _vendorRatingService;

        public VendorRatingController(IVendorRatingService vendorRatingService)
        {
            _vendorRatingService = vendorRatingService;
        }

        /// <summary>
        /// Submit a rating and comment for a vendor
        /// </summary>
        /// <param name="vendorId">The ID of the vendor</param>
        /// <param name="ratingDTO">The rating and comment</param>
        /// <returns>Confirmation of the rating submission</returns>
        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost("submit/{vendorId}")]
        public async Task<IActionResult> SubmitRating(
            string vendorId,
            [FromBody] SubmitRatingRequestDTO ratingDTO
        )
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized(new { message = "User is not authenticated" });

            var result = await _vendorRatingService.SubmitRatingAsync(
                vendorId,
                ratingDTO,
                customerId
            );
            if (!result)
                return BadRequest(new { message = "Failed to submit rating" });

            return Ok(new { message = "Rating submitted successfully and awaiting approval" });
        }

        /// <summary>
        /// Approve a vendor rating
        /// </summary>
        /// <param name="ratingId">The ID of the rating to approve</param>
        /// <returns>Confirmation of the approval</returns>
        [Authorize(Policy = "AdminOrCSRPolicy")]
        [HttpPut("approve/{ratingId}")]
        public async Task<IActionResult> ApproveRating(string ratingId)
        {
            var result = await _vendorRatingService.ApproveRatingAsync(ratingId);
            if (!result)
                return BadRequest(new { message = "Failed to approve rating" });

            return Ok(new { message = "Rating approved successfully" });
        }

        /// <summary>
        /// Get the profile of a vendor, including ratings and comments
        /// </summary>
        /// <param name="vendorId">The ID of the vendor</param>
        /// <returns>Vendor profile with ratings</returns>
        [HttpGet("profile/{vendorId}")]
        public async Task<IActionResult> GetVendorProfile(string vendorId)
        {
            var vendorProfile = await _vendorRatingService.GetVendorProfileAsync(vendorId);
            if (vendorProfile == null)
                return NotFound(new { message = "Vendor not found" });

            return Ok(vendorProfile);
        }
    }
}
