/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the ICategoryService interface, which provides
 * functionality for managing product categories in the e-commerce system.
 * It includes methods for retrieving, creating, updating, and deleting categories,
 * as well as retrieving the category hierarchy and child categories.
 * Date Created: 2024/09/28
 */

using System;
using ECommerceBackend.DTOs.Request.Category;
using ECommerceBackend.DTOs.Response.Category;

namespace ECommerceBackend.Service.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves the entire category tree structure.
        /// </summary>
        /// <returns>A list of CategoryResponseDTO representing the category hierarchy</returns>
        List<CategoryResponseDTO> GetCategoryTree();

        /// <summary>
        /// Retrieves a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns>A CategoryResponseDTO containing the category details</returns>
        CategoryResponseDTO GetCategoryById(string id);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryRequestDTO">The details of the category to be created</param>
        /// <returns>A CategoryResponseDTO containing the newly created category details</returns>
        CategoryResponseDTO CreateCategory(CategoryRequestDTO categoryRequestDTO);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to be updated</param>
        /// <param name="categoryRequestDTO">The updated category details</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        bool UpdateCategory(string id, CategoryRequestDTO categoryRequestDTO);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to be deleted</param>
        /// <returns>True if the category was deleted successfully, false otherwise</returns>
        bool DeleteCategory(string id);

        /// <summary>
        /// Retrieves all child categories of a specific parent category.
        /// </summary>
        /// <param name="parentId">The ID of the parent category</param>
        /// <returns>A list of CategoryResponseDTO representing the child categories</returns>
        List<CategoryResponseDTO> GetChildCategories(string parentId);
    }
}
