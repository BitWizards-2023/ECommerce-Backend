/*
 * File: CategoryMapper.cs
 * Description: Provides mapping methods to convert Category DTOs to Category models and vice versa.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-10-01
 */

using System;
using ECommerceBackend.DTOs.Request.Category;
using ECommerceBackend.DTOs.Response.Category;
using ECommerceBackend.Models.Entities;

namespace ECommerceBackend.Helpers.Mapper
{
    public class CategoryMapper
    {
        /// <summary>
        /// Converts a CategoryRequestDTO to a Category model.
        /// </summary>
        /// <param name="dto">The CategoryRequestDTO containing category details.</param>
        /// <returns>A Category model populated with the provided details.</returns>
        // Maps the CategoryRequestDTO to a Category model, setting default values for creation and update timestamps.
        public static Category ToCategoryModel(CategoryRequestDTO dto)
        {
            return new Category
            {
                Name = dto.Name,
                ParentId = string.IsNullOrWhiteSpace(dto.ParentId) ? null : dto.ParentId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Converts a Category model to a CategoryResponseDTO.
        /// </summary>
        /// <param name="category">The Category model to be converted.</param>
        /// <returns>A CategoryResponseDTO populated with the category's details.</returns>
        // Transforms the Category model into a CategoryResponseDTO for response purposes.
        public static CategoryResponseDTO ToCategoryResponseDTO(Category category)
        {
            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ParentId = category.ParentId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                ImageUrl = category.ImageUrl,
            };
        }
    }
}
