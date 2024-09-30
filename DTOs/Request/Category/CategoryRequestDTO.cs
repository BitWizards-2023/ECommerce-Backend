using System;

namespace ECommerceBackend.DTOs.Request.Category;

public class CategoryRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string? ParentId { get; set; } // Null for root categories
    public bool IsActive { get; set; } = true;
}
