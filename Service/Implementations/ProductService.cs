using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceBackend.Data.Contexts;
using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Product;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Models.Entities;
using ECommerceBackend.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceBackend.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly MongoDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductService(MongoDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Method to get products asynchronously
        public async Task<List<ProductResponseDTO>> GetProductsAsync()
        {
            var products = await _context.Products.Find(p => p.IsActive).ToListAsync();
            return products.Select(ProductMapper.ToProductResponseDTO).ToList();
        }

        // Method to search products asynchronously
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

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                var categoryFilter = filterBuilder.AnyEq(p => p.CategoryIds, categoryId);
                filter = filter & categoryFilter;
            }

            if (!string.IsNullOrWhiteSpace(vendorId))
            {
                var vendorFilter = filterBuilder.Eq(p => p.VendorId, vendorId);
                filter = filter & vendorFilter;
            }

            filter = filter & filterBuilder.Eq(p => p.IsActive, true);

            var products = await _context
                .Products.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return products.Select(ProductMapper.ToProductResponseDTO).ToList();
        }

        // Method to get a product by ID asynchronously
        public async Task<ProductResponseDTO> GetProductByIdAsync(string id)
        {
            var product = await _context
                .Products.Find(p => p.Id == id && p.IsActive)
                .FirstOrDefaultAsync();
            return product != null ? ProductMapper.ToProductResponseDTO(product) : null;
        }

        // Method to create a product asynchronously
        public async Task<ProductResponseDTO> CreateProductAsync(
            ProductRequestDTO productRequestDTO,
            string vendorId
        )
        {
            var product = ProductMapper.ToProductModel(productRequestDTO, vendorId);
            await _context.Products.InsertOneAsync(product);
            return ProductMapper.ToProductResponseDTO(product);
        }

        // Method to update a product asynchronously
        public async Task<bool> UpdateProductAsync(
            string id,
            ProductRequestDTO productRequestDTO,
            string vendorId
        )
        {
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

            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );

            return result.ModifiedCount > 0;
        }

        // Method to delete (soft delete) a product asynchronously
        public async Task<bool> DeleteProductAsync(string id, string vendorId)
        {
            var canDelete = await CanDeleteProductAsync(id, vendorId);

            if (!canDelete)
                return false;

            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, false);
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        // Method to activate a product asynchronously
        public async Task<bool> ActivateProductAsync(string id, string vendorId)
        {
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, true);
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        // Method to deactivate a product asynchronously
        public async Task<bool> DeactivateProductAsync(string id, string vendorId)
        {
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, false);
            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        // **New Methods for Inventory Management**

        // Method to get the stock level of a specific product
        public async Task<int> GetProductStockLevelAsync(string productId)
        {
            var product = await _context
                .Products.Find(p => p.Id == productId && p.IsActive)
                .FirstOrDefaultAsync();

            return product?.StockLevel ?? 0;
        }

        // Method to get the stock levels of all products
        public async Task<List<ProductStockResponseDTO>> GetAllProductStockLevelsAsync()
        {
            var products = await _context
                .Products.Find(p => p.IsActive)
                .Project(p => new ProductStockResponseDTO
                {
                    ProductId = p.Id,
                    StockLevel = p.StockLevel,
                })
                .ToListAsync();

            return products;
        }

        // Method to update the stock level of a product
        public async Task<bool> UpdateProductStockLevelAsync(
            string productId,
            int stockLevel,
            string vendorId
        )
        {
            var update = Builders<Product>
                .Update.Set(p => p.StockLevel, stockLevel)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _context.Products.UpdateOneAsync(
                p => p.Id == productId && p.VendorId == vendorId,
                update
            );

            return result.ModifiedCount > 0;
        }

        // Method to check if a product can be deleted (not part of any pending orders)
        public async Task<bool> CanDeleteProductAsync(string productId, string vendorId)
        {
            // Check if the product exists and belongs to the vendor
            var productExists = await _context
                .Products.Find(p => p.Id == productId && p.VendorId == vendorId && p.IsActive)
                .AnyAsync();

            if (!productExists)
                return false;

            // Check if the product is part of any pending orders
            var pendingOrders = await _context
                .Orders.Find(o =>
                    o.Items.Any(i => i.ProductId == productId) && o.Status == "Processing"
                )
                .AnyAsync();

            return !pendingOrders;
        }
    }
}
