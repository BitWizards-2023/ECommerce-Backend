/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the DownloadFileResponseDto, which is used to return
 * the details of a file being downloaded, including the blob name, content type, and
 * file stream in response to blob-related API requests.
 * Date Created: 2024/09/28
 */

using System.IO;

namespace ECommerceBackend.DTOs.Response.Upload
{
    /// <summary>
    /// Represents the details of a file being downloaded in response to blob-related API requests.
    /// </summary>
    public class DownloadFileResponseDto
    {
        /// <summary>
        /// Gets or sets the name of the blob being downloaded.
        /// </summary>
        public string BlobName { get; set; }

        /// <summary>
        /// Gets or sets the content type of the file (e.g., "application/pdf").
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file stream containing the file content.
        /// </summary>
        public Stream FileStream { get; set; }
    }
}
