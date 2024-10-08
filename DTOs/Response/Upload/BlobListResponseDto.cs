/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the BlobListResponseDto, which is used to return a list
 * of blob names in response to blob-related API requests, typically when listing all the
 * blobs stored in a container.
 * Date Created: 2024/09/28
 */

using System.Collections.Generic;

namespace ECommerceBackend.DTOs.Response.Upload
{
    /// <summary>
    /// Represents the list of blob names returned in response to blob-related API requests.
    /// </summary>
    public class BlobListResponseDto
    {
        /// <summary>
        /// Gets or sets the list of blob names.
        /// </summary>
        public List<string> BlobNames { get; set; }
    }
}
