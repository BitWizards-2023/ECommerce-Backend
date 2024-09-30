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
    }
}
