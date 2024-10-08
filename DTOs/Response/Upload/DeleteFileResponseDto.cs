/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the DeleteFileResponseDto, which is used to return a message
 * confirming the result of a file deletion operation in response to blob-related API requests.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Response.Upload
{
    /// <summary>
    /// Represents the response message returned after a file deletion operation.
    /// </summary>
    public class DeleteFileResponseDto
    {
        /// <summary>
        /// Gets or sets the message confirming the result of the file deletion.
        /// </summary>
        public string Message { get; set; }
    }
}
