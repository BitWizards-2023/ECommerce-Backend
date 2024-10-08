/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the CategoryRequestDTO, which is used to capture the details
 * for creating or updating a product category in the e-commerce system. It includes properties
 * for the category name, parent category, activation status, and an optional image URL.
 * Date Created: 2024/09/28
 */

using System;

namespace ECommerceBackend.DTOs.Request.Category
{
    /// <summary>
    /// Represents a request to create or update a category in the e-commerce system.
    /// </summary>
    public class CategoryRequestDTO
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the parent category. This can be null for root categories.
        /// </summary>
        public string? ParentId { get; set; } // Null for root categories

        /// <summary>
        /// Gets or sets a value indicating whether the category is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the optional image URL associated with the category.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
