/*
 * File: CategoryService.cs
 * Description: Implements category-related operations such as creation, retrieval, updating, and deletion using MongoDB.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-10-01
 */

using System;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Category;
using ECommerceBackend.DTOs.Response.Category;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Models;
using ECommerceBackend.Models.Entities;
using ECommerceBackend.Service.Interfaces;
using MongoDB.Driver;

namespace ECommerceBackend.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly MongoDbContext _context;
        private readonly IConfiguration _configuration;

        // Initializes the CategoryService with the necessary MongoDB context and configuration settings.
        public CategoryService(MongoDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Creates a new category based on the provided request DTO.
        /// </summary>
        /// <param name="categoryRequestDTO">The DTO containing category creation details.</param>
        /// <returns>The created category as a CategoryResponseDTO.</returns>
        // Creates a new category in the database using the provided CategoryRequestDTO and returns the created category as a response DTO.
        public CategoryResponseDTO CreateCategory(CategoryRequestDTO categoryRequestDTO)
        {
            var category = CategoryMapper.ToCategoryModel(categoryRequestDTO);
            _context.Categories.InsertOne(category);
            return CategoryMapper.ToCategoryResponseDTO(category);
        }

        /// <summary>
        /// Deletes a category by setting its IsActive flag to false.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        // Deletes the specified category by updating its IsActive property to false, effectively marking it as inactive.
        public bool DeleteCategory(string id)
        {
            var updateDefinition = Builders<Category>.Update.Set(c => c.IsActive, false);
            var result = _context.Categories.UpdateOne(c => c.Id == id, updateDefinition);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The requested category as a CategoryResponseDTO if found; otherwise, null.</returns>
        // Retrieves a category from the database based on the provided ID and returns it as a response DTO if it exists.
        public CategoryResponseDTO GetCategoryById(string id)
        {
            var category = _context.Categories.Find(c => c.Id == id).FirstOrDefault();
            return category != null ? CategoryMapper.ToCategoryResponseDTO(category) : null;
        }

        /// <summary>
        /// Retrieves the entire category tree, organizing categories hierarchically based on parent-child relationships.
        /// </summary>
        /// <returns>A list of CategoryResponseDTO representing the hierarchical category tree.</returns>
        // Constructs and returns a hierarchical category tree by mapping each category's parent-child relationships.
        public List<CategoryResponseDTO> GetCategoryTree()
        {
            var categories = _context.Categories.Find(FilterDefinition<Category>.Empty).ToList();
            var categoryMap = categories.ToDictionary(
                c => c.Id,
                c => CategoryMapper.ToCategoryResponseDTO(c)
            );

            foreach (var category in categoryMap.Values)
            {
                if (
                    !string.IsNullOrWhiteSpace(category.ParentId)
                    && categoryMap.ContainsKey(category.ParentId)
                )
                {
                    categoryMap[category.ParentId].Children.Add(category);
                }
            }

            return categoryMap.Values.Where(c => string.IsNullOrWhiteSpace(c.ParentId)).ToList();
        }

        /// <summary>
        /// Retrieves all child categories under a specific parent category.
        /// </summary>
        /// <param name="parentId">The ID of the parent category.</param>
        /// <returns>A list of CategoryResponseDTO representing the child categories.</returns>
        // Fetches and returns all active child categories associated with the specified parent category ID.
        public List<CategoryResponseDTO> GetChildCategories(string parentId)
        {
            var categories = _context
                .Categories.Find(c => c.ParentId == parentId && c.IsActive)
                .ToList();
            return categories.Select(CategoryMapper.ToCategoryResponseDTO).ToList();
        }

        /// <summary>
        /// Updates an existing category with the provided details.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryRequestDTO">The DTO containing updated category details.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        // Updates the specified category's details in the database using the provided CategoryRequestDTO.
        public bool UpdateCategory(string id, CategoryRequestDTO categoryRequestDTO)
        {
            var updateDefinition = Builders<Category>
                .Update.Set(c => c.Name, categoryRequestDTO.Name)
                .Set(c => c.IsActive, categoryRequestDTO.IsActive)
                .Set(c => c.ParentId, categoryRequestDTO.ParentId)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);

            var result = _context.Categories.UpdateOne(c => c.Id == id, updateDefinition);

            return result.ModifiedCount > 0;
        }
    }
}
