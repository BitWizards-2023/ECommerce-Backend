using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceBackend.Models.Entities
{
    public class Category
    {
        // Unique identifier for the category
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        // Name of the category
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        // Indicates whether the category is active
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        // Reference to the parent category (null for root categories)
        [BsonElement("parentId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentId { get; set; } = null;

        // Timestamp when the category was created
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Timestamp when the category was last updated
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
