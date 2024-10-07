using ECommerceBackend.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("Role")]
        public string Role { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("LastName")]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("Address")]
        public Address Address { get; set; } = new Address();

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("ProfilePic")]
        public string ProfilePic { get; set; } = string.Empty;

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("IsActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("RefreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [BsonElement("RefreshTokenExpiryTime")]
        public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.UtcNow;

        [BsonElement("PasswordResetToken")]
        public string PasswordResetToken { get; set; } = string.Empty;

        [BsonElement("ResetTokenExpiryTime")]
        public DateTime ResetTokenExpiryTime { get; set; } = DateTime.UtcNow;

        // Additional fields for vendor ratings
        [BsonElement("Ratings")]
        public List<VendorRating> Ratings { get; set; } = new List<VendorRating>();

        [BsonElement("AverageRating")]
        public double AverageRating { get; set; } = 0.0;

        [BsonElement("TotalReviews")]
        public int TotalReviews { get; set; } = 0;
    }

    public class Address
    {
        [BsonElement("Street")]
        public string Street { get; set; } = string.Empty;

        [BsonElement("City")]
        public string City { get; set; } = string.Empty;

        [BsonElement("State")]
        public string State { get; set; } = string.Empty;

        [BsonElement("PostalCode")]
        public string PostalCode { get; set; } = string.Empty;

        [BsonElement("Country")]
        public string Country { get; set; } = string.Empty;

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
