/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the ProductDetailsDTO, which is used to return details about a product
 * associated with an order item. It includes properties for the product name, description, price,
 * and a list of image URLs.
 * Date Created: 2024/09/28
 */

using System.Collections.Generic;

public class ProductDetailsDTO
{
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
    /// Gets or sets the list of image URLs associated with the product.
    /// </summary>
    public List<string> Images { get; set; }
}
