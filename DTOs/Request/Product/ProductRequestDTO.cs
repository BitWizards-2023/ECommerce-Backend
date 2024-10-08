/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the ProductRequestDTO and ProductAttributeDTO, which are used
 * to capture the details for creating or updating a product in the e-commerce system. It includes
 * product code, name, description, price, category IDs, images, attributes, stock levels, and more.
 * Date Created: 2024/09/28
 */

using System;
using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Request.Product
{
    /// <summary>
    /// Represents the details required to create or update a product in the e-commerce system.
    /// </summary>
    public class ProductRequestDTO
    {
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
        /// Gets or sets the list of image URLs for the product.
        /// </summary>
        public List<string> Images { get; set; }

        /// <summary>
        /// Gets or sets the list of attributes (e.g., color, size) for the product.
        /// </summary>
        public List<ProductAttributeDTO> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the stock level of the product.
        /// </summary>
        public int StockLevel { get; set; }

        /// <summary>
        /// Gets or sets the low stock threshold at which a product is considered low in stock.
        /// </summary>
        public int LowStockThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is featured.
        /// </summary>
        public bool IsFeatured { get; set; }
    }

    /// <summary>
    /// Represents an attribute of a product (e.g., color, size).
    /// </summary>
    public class ProductAttributeDTO
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
