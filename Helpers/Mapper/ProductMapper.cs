using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Product;
using ECommerceBackend.Models.Entities;

namespace ECommerceBackend.Helpers.Mapper
{
    public static class ProductMapper
    {
        public static Product ToProductModel(ProductRequestDTO dto, string vendorId)
        {
            return new Product
            {
                ProductCode = dto.ProductCode,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryIds = dto.CategoryIds,
                Images = dto.Images,
                StockLevel = dto.StockLevel,
                LowStockThreshold = dto.LowStockThreshold,
                IsFeatured = dto.IsFeatured,
                IsActive = true,
                VendorId = vendorId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Attributes = dto
                    .Attributes.Select(a => new ProductAttribute { Name = a.Name, Value = a.Value })
                    .ToList(),
            };
        }

        public static ProductResponseDTO ToProductResponseDTO(Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryIds = product.CategoryIds,
                VendorId = product.VendorId,
                Images = product.Images,
                Attributes = product
                    .Attributes.Select(a => new ProductAttributeResponseDTO
                    {
                        Name = a.Name,
                        Value = a.Value,
                    })
                    .ToList(),
                StockLevel = product.StockLevel,
                LowStockThreshold = product.LowStockThreshold,
                IsFeatured = product.IsFeatured,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                AverageRating = product.AverageRating,
                RatingsCount = product.RatingsCount,
            };
        }
    }
}
