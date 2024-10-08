/*
 * File: CategoryController.cs
 * Description: Handles HTTP requests related to category operations such as retrieval, creation, updating, and deletion.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-10-01
 */

using ECommerceBackend.DTOs.Request.Category;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.DTOs.Response.Category;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        // Initializes the CategoryController with the necessary category service.
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves the entire category tree.
        /// </summary>
        /// <returns>A list of CategoryResponseDTO representing the category tree.</returns>
        // Retrieves the complete category hierarchy and returns it as a response.
        [HttpGet]
        public IActionResult GetCategoryTree()
        {
            var categoryTree = _categoryService.GetCategoryTree();
            return Ok(
                new ResponseDTO<List<CategoryResponseDTO>>(
                    true,
                    "Category tree retrieved successfully",
                    categoryTree
                )
            );
        }

        /// <summary>
        /// Retrieves a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The requested category if found; otherwise, a not found response.</returns>
        // Fetches a category based on the provided ID and returns the result.
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(string id)
        {
            var category = _categoryService.GetCategoryById(id);
            return category != null
                ? Ok(
                    new ResponseDTO<CategoryResponseDTO>(
                        true,
                        "Category retrieved successfully",
                        category
                    )
                )
                : NotFound(new ResponseDTO<string>(false, "Category not found", null));
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryRequestDTO">The details of the category to create.</param>
        /// <returns>The created category with its assigned ID.</returns>
        // Creates a new category using the provided request data and returns the created category.
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryRequestDTO categoryRequestDTO)
        {
            var category = _categoryService.CreateCategory(categoryRequestDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Updates an existing category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryRequestDTO">The updated category details.</param>
        /// <returns>No content if successful; otherwise, a not found response.</returns>
        // Updates an existing category with the provided ID and request data.
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(
            string id,
            [FromBody] CategoryRequestDTO categoryRequestDTO
        )
        {
            var result = _categoryService.UpdateCategory(id, categoryRequestDTO);
            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Category not found or update failed", null)
                );
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>No content if successful; otherwise, a not found response.</returns>
        // Deletes the category with the specified ID from the system.
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(string id)
        {
            var result = _categoryService.DeleteCategory(id);
            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Category not found or deletion failed", null)
                );
        }

        /// <summary>
        /// Retrieves child categories of a specific category.
        /// </summary>
        /// <param name="id">The ID of the parent category.</param>
        /// <returns>A list of child CategoryResponseDTO.</returns>
        // Fetches all child categories under the specified parent category.
        [HttpGet("{id}/children")]
        public IActionResult GetChildCategories(string id)
        {
            var childCategories = _categoryService.GetChildCategories(id);
            return Ok(
                new ResponseDTO<List<CategoryResponseDTO>>(
                    true,
                    "Child categories retrieved successfully",
                    childCategories
                )
            );
        }
    }
}
