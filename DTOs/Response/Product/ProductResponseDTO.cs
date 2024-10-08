/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the ProductResponseDTO and ProductAttributeResponseDTO, which are used to
 * return the details of a product in response to product-related API requests. It includes properties such
 * as product code, name, description, price, category IDs, vendor ID, images, attributes, stock level, and ratings.
 * Date Created: 2024/09/28
 */

using System;
using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Product
{
    /// <summary>
    /// Represents the details of a product returned in response to product-related API requests.
    /// </summary>
    public class ProductResponseDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the product code (unique identifier).
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the list of category IDs the product belongs to.
        /// </summary>
        public List<string> CategoryIds { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vendor selling the product.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Gets or sets the list of image URLs associated with the product.
        /// </summary>
        public List<string> Images { get; set; }

        /// <summary>
        /// Gets or sets the list of attributes associated with the product.
        /// </summary>
        public List<ProductAttributeResponseDTO> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the current stock level of the product.
        /// </summary>
        public int StockLevel { get; set; }

        /// <summary>
        /// Gets or sets the stock level threshold at which the product is considered low in stock.
        /// </summary>
        public int LowStockThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is featured.
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the date and time the product was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time the product was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the average rating of the product.
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the total number of ratings the product has received.
        /// </summary>
        public int RatingsCount { get; set; }
    }

    /// <summary>
    /// Represents the attributes of a product (e.g., color, size) returned in response to product-related API requests.
    /// </summary>
    public class ProductAttributeResponseDTO
    {
        /// <summary>
        /// Gets or sets the name of the attribute (e.g., "Color").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the attribute (e.g., "Red").
        /// </summary>
        public string Value { get; set; }
    }
}
