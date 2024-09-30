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

        public List<ProductResponseDTO> GetProducts()
        {
            var products = _context.Products.Find(p => p.IsActive).ToList();
            return products.Select(ProductMapper.ToProductResponseDTO).ToList();
        }

        public List<ProductResponseDTO> SearchProducts(
            string keyword,
            string categoryId,
            string vendorId,
            int pageNumber,
            int pageSize
        )
        {
            // Building the filter for searching
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            // Add keyword filter
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

            // Add categoryId filter (checking if any element in the CategoryIds list matches the given categoryId)
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                var categoryFilter = filterBuilder.AnyEq(p => p.CategoryIds, categoryId);
                filter = filter & categoryFilter;
            }

            // Add vendorId filter
            if (!string.IsNullOrWhiteSpace(vendorId))
            {
                var vendorFilter = filterBuilder.Eq(p => p.VendorId, vendorId);
                filter = filter & vendorFilter;
            }

            // Ensure the product is active
            filter = filter & filterBuilder.Eq(p => p.IsActive, true);

            // Retrieve products that match the search criteria with pagination
            var products = _context
                .Products.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToList();

            return products.Select(ProductMapper.ToProductResponseDTO).ToList();
        }

        public ProductResponseDTO GetProductById(string id)
        {
            var product = _context.Products.Find(p => p.Id == id).FirstOrDefault();
            return product != null ? ProductMapper.ToProductResponseDTO(product) : null;
        }

        public ProductResponseDTO CreateProduct(
            ProductRequestDTO productRequestDTO,
            string vendorId
        )
        {
            var product = ProductMapper.ToProductModel(productRequestDTO, vendorId);
            _context.Products.InsertOne(product);
            return ProductMapper.ToProductResponseDTO(product);
        }

        public bool UpdateProduct(string id, ProductRequestDTO productRequestDTO, string vendorId)
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

            var result = _context.Products.UpdateOne(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );

            return result.ModifiedCount > 0;
        }

        public bool DeleteProduct(string id, string vendorId)
        {
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, false);
            var result = _context.Products.UpdateOne(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        public bool ActivateProduct(string id, string vendorId)
        {
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, true);
            var result = _context.Products.UpdateOne(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }

        public bool DeactivateProduct(string id, string vendorId)
        {
            var updateDefinition = Builders<Product>.Update.Set(p => p.IsActive, false);
            var result = _context.Products.UpdateOne(
                p => p.Id == id && p.VendorId == vendorId,
                updateDefinition
            );
            return result.ModifiedCount > 0;
        }
    }
}
