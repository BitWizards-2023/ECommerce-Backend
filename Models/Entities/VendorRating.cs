using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceBackend.Models.Entities
{
    public class VendorRating
    {
        // Unique identifier for the rating
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        // Reference to the vendor being rated
        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } = string.Empty;

        // Reference to the customer who submitted the rating
        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = string.Empty;

        // Rating value (e.g., 1 to 5 stars)
        [BsonElement("rating")]
        public int Rating { get; set; }

        // Customer's comment about the vendor
        [BsonElement("comment")]
        public string Comment { get; set; } = string.Empty;

        // Timestamp when the rating was created
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Flag to indicate if the rating has been approved
        [BsonElement("isApproved")]
        public bool IsApproved { get; set; } = false;
    }
}
