/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the IProductService interface, which provides
 * functionality for managing products in the e-commerce system. It includes methods
 * for retrieving, creating, updating, activating, deactivating, and deleting products,
 * as well as searching products based on keywords, categories, and vendor IDs.
 * Date Created: 2024/09/28
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Product;

namespace ECommerceBackend.Service.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a list of all active products asynchronously.
        /// </summary>
        /// <returns>A list of ProductResponseDTO containing the product details</returns>
        Task<List<ProductResponseDTO>> GetProductsAsync();

        /// <summary>
        /// Searches for products based on keyword, category, and vendor with pagination.
        /// </summary>
        /// <param name="keyword">The keyword to search for (optional)</param>
        /// <param name="categoryId">The ID of the category to filter by (optional)</param>
        /// <param name="vendorId">The ID of the vendor to filter by (optional)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <param name="pageSize">The page size for pagination</param>
        /// <returns>A list of ProductResponseDTO containing the filtered product details</returns>
        Task<List<ProductResponseDTO>> SearchProductsAsync(
            string keyword,
            string categoryId,
            string vendorId,
            int pageNumber,
            int pageSize
        );

        /// <summary>
        /// Retrieves a specific product by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product</param>
        /// <returns>A ProductResponseDTO containing the product details</returns>
        Task<ProductResponseDTO> GetProductByIdAsync(string id);

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="productRequestDTO">The product details</param>
        /// <param name="vendorId">The ID of the vendor creating the product</param>
        /// <returns>A ProductResponseDTO containing the created product details</returns>
        Task<ProductResponseDTO> CreateProductAsync(
            ProductRequestDTO productRequestDTO,
            string vendorId
        );

        /// <summary>
        /// Updates an existing product asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to update</param>
        /// <param name="productRequestDTO">The updated product details</param>
        /// <param name="vendorId">The ID of the vendor updating the product</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        Task<bool> UpdateProductAsync(
            string id,
            ProductRequestDTO productRequestDTO,
            string vendorId
        );

        /// <summary>
        /// Soft deletes a product (sets IsActive to false) asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to delete</param>
        /// <param name="vendorId">The ID of the vendor deleting the product</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        Task<bool> DeleteProductAsync(string id, string vendorId);

        /// <summary>
        /// Activates a product by setting IsActive to true asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to activate</param>
        /// <param name="vendorId">The ID of the vendor activating the product</param>
        /// <returns>True if the activation was successful, false otherwise</returns>
        Task<bool> ActivateProductAsync(string id, string vendorId);

        /// <summary>
        /// Deactivates a product by setting IsActive to false asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to deactivate</param>
        /// <param name="vendorId">The ID of the vendor deactivating the product</param>
        /// <returns>True if the deactivation was successful, false otherwise</returns>
        Task<bool> DeactivateProductAsync(string id, string vendorId);
    }
}
