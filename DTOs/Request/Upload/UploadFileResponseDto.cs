/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the UploadFileRequestDto, which is used to capture
 * the details required for uploading a file, including the file itself. It includes
 * validation to ensure the file is required.
 * Date Created: 2024/09/28
 */

using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTOs.Request.Upload
{
    /// <summary>
    /// Represents the details required to upload a file.
    /// </summary>
    public class UploadFileRequestDto
    {
        /// <summary>
        /// Gets or sets the file to be uploaded. The file is required.
        /// </summary>
        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; }
    }
}
