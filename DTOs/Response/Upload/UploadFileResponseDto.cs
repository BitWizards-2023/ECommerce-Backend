/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the UploadFileResponseDto, which is used to return
 * details after a file upload operation, including the URL of the uploaded file and
 * the blob name in response to file upload-related API requests.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Response
{
    /// <summary>
    /// Represents the response returned after a file upload operation.
    /// </summary>
    public class UploadFileResponseDto
    {
        /// <summary>
        /// Gets or sets the URL of the uploaded file.
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the blob where the file is stored.
        /// </summary>
        public string BlobName { get; set; }
    }
}
