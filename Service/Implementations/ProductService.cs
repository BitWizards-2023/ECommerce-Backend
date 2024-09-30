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
            var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
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
    }
}
