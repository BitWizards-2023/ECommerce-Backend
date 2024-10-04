using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceBackend.DTOs.Request.Product;
using ECommerceBackend.DTOs.Response.Auth;
using ECommerceBackend.DTOs.Response.Product;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(
                new ResponseDTO<List<ProductResponseDTO>>(
                    true,
                    "Products retrieved successfully",
                    products
                )
            );
        }

        // Endpoint for searching products with optional filtering by keyword, categoryId, and vendorId
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string keyword = "",
            [FromQuery] string categoryId = "",
            [FromQuery] string vendorId = "",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(
                    new ResponseDTO<string>(false, "Invalid pagination parameters", null)
                );
            }

            var products = await _productService.SearchProductsAsync(
                keyword,
                categoryId,
                vendorId,
                pageNumber,
                pageSize
            );

            if (products == null || products.Count == 0)
            {
                return NotFound(
                    new ResponseDTO<string>(
                        false,
                        "No products found matching the search criteria",
                        null
                    )
                );
            }

            return Ok(
                new ResponseDTO<List<ProductResponseDTO>>(
                    true,
                    "Products retrieved successfully",
                    products
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product != null
                ? Ok(
                    new ResponseDTO<ProductResponseDTO>(
                        true,
                        "Product retrieved successfully",
                        product
                    )
                )
                : NotFound(new ResponseDTO<string>(false, "Product not found", null));
        }

        [Authorize(Policy = "VendorPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] ProductRequestDTO productRequestDTO
        )
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _productService.CreateProductAsync(productRequestDTO, vendorId);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [Authorize(Policy = "VendorPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            string id,
            [FromBody] ProductRequestDTO productRequestDTO
        )
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.UpdateProductAsync(id, productRequestDTO, vendorId);
            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or update failed", null)
                );
        }

        [Authorize(Policy = "VendorPolicy")]
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.DeleteProductAsync(id, vendorId);
            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or deletion failed", null)
                );
        }

        [Authorize(Policy = "VendorPolicy")]
        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateProduct(string id)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.ActivateProductAsync(id, vendorId);
            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or activation failed", null)
                );
        }

        [Authorize(Policy = "VendorPolicy")]
        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateProduct(string id)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.DeactivateProductAsync(id, vendorId);
            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or deactivation failed", null)
                );
        }
    }
}
