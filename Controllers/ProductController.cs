/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the ProductController class,
 * which provides functionality for managing products in the ECommerceBackend application.
 * It includes methods for creating, retrieving, updating, deleting, and searching for products.
 * Date Created: 2024/09/18
 */

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

        /// <summary>
        /// Constructor for ProductController, injecting the ProductService.
        /// </summary>
        /// <param name="productService">Service for managing product operations</param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            // Retrieve all products from the service
            var products = await _productService.GetProductsAsync();
            return Ok(
                new ResponseDTO<List<ProductResponseDTO>>(
                    true,
                    "Products retrieved successfully",
                    products
                )
            );
        }

        /// <summary>
        /// Searches for products with optional filters by keyword, category, and vendor.
        /// </summary>
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

            // Search products based on provided filters
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

        /// <summary>
        /// Retrieves a specific product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            // Retrieve the product by ID from the service
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

        /// <summary>
        /// Creates a new product (Vendor only).
        /// </summary>
        [Authorize(Policy = "VendorPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            [FromBody] ProductRequestDTO productRequestDTO
        )
        {
            // Extract vendor ID from the user's claims
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create a new product for the vendor
            var product = await _productService.CreateProductAsync(productRequestDTO, vendorId);

            // Return the created product details
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Updates an existing product (Vendor only).
        /// </summary>
        [Authorize(Policy = "VendorPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            string id,
            [FromBody] ProductRequestDTO productRequestDTO
        )
        {
            // Extract vendor ID from the user's claims
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Attempt to update the product for the vendor
            var result = await _productService.UpdateProductAsync(id, productRequestDTO, vendorId);

            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or update failed", null)
                );
        }

        /// <summary>
        /// Deletes a product (Vendor or Admin).
        /// </summary>
        [Authorize(Policy = "VendorOrAdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            // Extract vendor ID from the user's claims
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Attempt to delete the product for the vendor or admin
            var result = await _productService.DeleteProductAsync(id, vendorId);

            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or deletion failed", null)
                );
        }

        /// <summary>
        /// Activates a product (Vendor or Admin).
        /// </summary>
        [Authorize(Policy = "VendorOrAdminPolicy")]
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateProduct(string id)
        {
            // Extract vendor ID from the user's claims
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Attempt to activate the product for the vendor or admin
            var result = await _productService.ActivateProductAsync(id, vendorId);

            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or activation failed", null)
                );
        }

        /// <summary>
        /// Deactivates a product (Vendor or Admin).
        /// </summary>
        [Authorize(Policy = "VendorOrAdminPolicy")]
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateProduct(string id)
        {
            // Extract vendor ID from the user's claims
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Attempt to deactivate the product for the vendor or admin
            var result = await _productService.DeactivateProductAsync(id, vendorId);

            return result
                ? NoContent()
                : NotFound(
                    new ResponseDTO<string>(false, "Product not found or deactivation failed", null)
                );
        }
    }
}
