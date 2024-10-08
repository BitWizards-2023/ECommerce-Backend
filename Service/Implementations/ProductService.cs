/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the ProductService class,
 * which provides functionality for managing products in the ECommerceBackend application.
 * It includes methods for retrieving, creating, updating, activating, deactivating,
 * and soft-deleting products, as well as searching and filtering products based on keywords,
 * categories, and vendor IDs.
 * Date Created: 2024/09/18
 */

using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Product;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Models;
using ECommerceBackend.Models.Entities;
using ECommerceBackend.Service.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceBackend.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly MongoDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for ProductService, injecting the MongoDB context and configuration.
        /// </summary>
        /// <param name="context">The MongoDB context</param>
        /// <param name="configuration">The application configuration</param>
        public ProductService(MongoDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Retrieves a list of all active products asynchronously.
        /// </summary>
        public async Task<List<ProductResponseDTO>> GetProductsAsync()
        {
            // Retrieve all active products from the database
            var products = await _context.Products.Find(p => p.IsActive).ToListAsync();
            return products.Select(ProductMapper.ToProductResponseDTO).ToList();
        }

        /// <summary>
        /// Searches for products based on a keyword, category, or vendor ID asynchronously.
        /// </summary>
        public async Task<List<ProductResponseDTO>> SearchProductsAsync(
            string keyword,
            string categoryId,
            string vendorId,
            int pageNumber,
            int pageSize
        )
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            // Filter by keyword if provided
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var keywordFilter = filterBuilder.Or(
                    filterBuilder.Regex(p => p.Name, new BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex(
                        p => p.Description,
                        new BsonRegularExpression(keyword, "i")
                    ),
                    filterBuilder.Regex(p => p.ProductCode, new BsonRegularExpression(keyword, "i"))
                );
                filter = filter & keywordFilter;
            }

            // Filter by category if provided
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                var categoryFilter = filterBuilder.AnyEq(p => p.CategoryIds, categoryId);
                filter = filter & categoryFilter;
            }

            // Filter by vendor if provided
            if (!string.IsNullOrWhiteSpace(vendorId))
            {
                var vendorFilter = filterBuilder.Eq(p => p.VendorId, vendorId);
                filter = filter & vendorFilter;
            }

            // Ensure only active products are retrieved
            filter = filter & filterBuilder.Eq(p => p.IsActive, true);

            // Apply pagination
            var products = await _context
                .Products.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return products.Select(ProductMapper.ToProductResponseDTO).ToList();
        }

        /// <summary>
        /// Retrieves a product by its ID asynchronously.
        /// </summary>
        public async Task<ProductResponseDTO> GetProductByIdAsync(string id)
        {
            // Retrieve the product by ID from the database
            var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
            return product != null ? ProductMapper.ToProductResponseDTO(product) : null;
        }

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        public async Task<ProductResponseDTO> CreateProductAsync(
            ProductRequestDTO productRequestDTO,
            string vendorId
        )
        {
            // Map the product DTO to a product entity and insert it into the database
            var product = ProductMapper.ToProductModel(productRequestDTO, vendorId);
            await _context.Products.InsertOneAsync(product);
            return ProductMapper.ToProductResponseDTO(product);
        }

        /// <summary>
        /// Updates an existing product asynchronously.
        /// </summary>
        public async Task<bool> UpdateProductAsync(
            string id,
            ProductRequestDTO productRequestDTO,
            string vendorId
        )
        {
            // Build the update definition for the product
            var updateDefinition = Builders<Product>
                .Update.Set(p => p.Name, productRequestDTO.Name)
                .Set(p => p.Description, productRequestDTO.Description)
                .Set(p => p.Price, productRequestDTO.Price)
                .Set(p => p.CategoryIds, productRequestDTO.CategoryIds)
                .Set(p => p.Images, productRequestDTO.Images)
                .Set(
                    p => p.Attributes,
                    productRequestDTO
                        .Attributes.Select(a => new ProductAttribute
                        {
                            Name = a.Name,
                            Value = a.Value,
                        })
                        .ToList()
                )
                .Set(p => p.StockLevel, productRequestDTO.StockLevel)
                .Set(p => p.LowStockThreshold, productRequestDTO.LowStockThreshold)
                .Set(p => p.IsFeatured, productRequestDTO.IsFeatured)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            // Update the product in the database
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );

            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Soft-deletes a product (sets IsActive to false) asynchronously.
        /// </summary>
        public async Task<bool> DeleteProductAsync(string id, string vendorId)
        {
            // Set the product as inactive (soft delete)
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, false);
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Activates a product (sets IsActive to true) asynchronously.
        /// </summary>
        public async Task<bool> ActivateProductAsync(string id, string vendorId)
        {
            // Set the product as active
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, true);
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deactivates a product (sets IsActive to false) asynchronously.
        /// </summary>
        public async Task<bool> DeactivateProductAsync(string id, string vendorId)
        {
            // Set the product as inactive
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, false);
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }
    }
}
