using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Product;

namespace ECommerceBackend.Service.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProductsAsync();
        Task<List<ProductResponseDTO>> SearchProductsAsync(
            string keyword,
            string categoryId,
            string vendorId,
            int pageNumber,
            int pageSize
        );
        Task<ProductResponseDTO> GetProductByIdAsync(string id);
        Task<ProductResponseDTO> CreateProductAsync(
            ProductRequestDTO productRequestDTO,
            string vendorId
        );
        Task<bool> UpdateProductAsync(
            string id,
            ProductRequestDTO productRequestDTO,
            string vendorId
        );
        Task<bool> DeleteProductAsync(string id, string vendorId);
        Task<bool> ActivateProductAsync(string id, string vendorId);
        Task<bool> DeactivateProductAsync(string id, string vendorId);

        // Gets the stock level of a specific product
        Task<int> GetProductStockLevelAsync(string productId);

        // Gets the stock levels of all products
        Task<List<ProductStockResponseDTO>> GetAllProductStockLevelsAsync();

        // Updates the stock level of a product
        Task<bool> UpdateProductStockLevelAsync(string productId, int stockLevel, string vendorId);

        // Checks if a product can be deleted (not part of any pending orders)
        Task<bool> CanDeleteProductAsync(string productId, string vendorId);
    }
}
