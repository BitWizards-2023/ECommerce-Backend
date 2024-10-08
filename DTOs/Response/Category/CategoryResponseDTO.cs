/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the CategoryResponseDTO, which is used to return details
 * about a product category in response to category-related API requests. It includes
 * properties for the category ID, name, active status, parent ID, child categories, and timestamps.
 * Date Created: 2024/09/28
 */

using System;
using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Category
{
    /// <summary>
    /// Represents the details of a category returned in response to category-related API requests.
    /// </summary>
    public class CategoryResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the category is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent category (if any).
        /// </summary>
        public string? ParentId { get; set; } = null;

        /// <summary>
        /// Gets or sets the list of child categories (subcategories).
        /// </summary>
        public List<CategoryResponseDTO> Children { get; set; } = new List<CategoryResponseDTO>();

        /// <summary>
        /// Gets or sets the date and time when the category was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the category was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the URL of the category image (if any).
        /// </summary>
        public string? ImageUrl { get; set; } = string.Empty;
    }
}
