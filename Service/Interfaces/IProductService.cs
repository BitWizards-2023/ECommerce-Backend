using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Product;

namespace ECommerceBackend.Service.Interfaces
{
    public interface IProductService
    {
        List<ProductResponseDTO> GetProducts();
        List<ProductResponseDTO> SearchProducts(
            string keyword,
            string categoryId,
            string vendorId,
            int pageNumber,
            int pageSize
        );
        ProductResponseDTO GetProductById(string id);
        ProductResponseDTO CreateProduct(ProductRequestDTO productRequestDTO, string vendorId);
        bool UpdateProduct(string id, ProductRequestDTO productRequestDTO, string vendorId);
        bool DeleteProduct(string id, string vendorId);
        bool ActivateProduct(string id, string vendorId);
        bool DeactivateProduct(string id, string vendorId);
    }
}
