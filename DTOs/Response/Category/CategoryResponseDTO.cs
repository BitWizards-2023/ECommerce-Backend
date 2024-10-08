using System;

namespace ECommerceBackend.DTOs.Response.Category;

public class CategoryResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ParentId { get; set; } = null;
    public List<CategoryResponseDTO> Children { get; set; } = new List<CategoryResponseDTO>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;
}
