/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file contains the implementation of the BlobController class,
 * which provides functionality for handling file uploads, downloads, deletion,
 * and listing of blobs in Azure Blob Storage for the ECommerceBackend application.
 * Date Created: 2024/09/18
 */

using ECommerceBackend.DTOs.Request.Upload;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IBloblService _blobService;
        private readonly ILogger<BlobController> _logger;

        /// <summary>
        /// Constructor for BlobController, injecting BlobService and Logger services.
        /// </summary>
        /// <param name="blobService">Service for managing Azure Blob Storage operations</param>
        /// <param name="logger">Logger for capturing logs</param>
        public BlobController(IBloblService blobService, ILogger<BlobController> logger)
        {
            _blobService = blobService;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a file to Azure Blob Storage.
        /// </summary>
        /// <param name="uploadDto">The file to upload.</param>
        /// <returns>The URL and blob name of the uploaded file.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadFileRequestDto uploadDto)
        {
            // Check if a file is provided in the request
            if (uploadDto.File == null || uploadDto.File.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                // Upload the file using the BlobService and map the response DTO
                string url = await _blobService.UploadFileAsync(uploadDto.File);
                var responseDto = BlobDTOsMapping.MapToUploadFileResponseDto(url);
                return Ok(responseDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid upload attempt.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Downloads a file from Azure Blob Storage.
        /// </summary>
        /// <param name="blobName">The name of the blob to download.</param>
        /// <returns>The file stream.</returns>
        [HttpGet("download/{blobName}")]
        public async Task<IActionResult> Download(string blobName)
        {
            // Validate that blob name is provided
            if (string.IsNullOrWhiteSpace(blobName))
                return BadRequest("Blob name is required.");

            try
            {
                // Download the file from BlobService and return it as a stream
                Stream fileStream = await _blobService.DownloadFileAsync(blobName);
                return File(fileStream, "application/octet-stream", blobName);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid download attempt.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file.");
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a file from Azure Blob Storage.
        /// </summary>
        /// <param name="blobName">The name of the blob to delete.</param>
        /// <returns>A message indicating the result.</returns>
        [HttpDelete("delete/{blobName}")]
        public async Task<IActionResult> Delete(string blobName)
        {
            // Validate that blob name is provided
            if (string.IsNullOrWhiteSpace(blobName))
                return BadRequest("Blob name is required.");

            try
            {
                // Delete the file from BlobService and map the response DTO
                bool deleted = await _blobService.DeleteFileAsync(blobName);
                var responseDto = BlobDTOsMapping.MapToDeleteFileResponseDto(deleted);
                if (deleted)
                    return Ok(responseDto);
                else
                    return NotFound(responseDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid delete attempt.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Lists all blobs in the container.
        /// </summary>
        /// <returns>A list of blob names.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            try
            {
                // List blobs from BlobService and map the response DTO
                var blobs = await _blobService.ListBlobsAsync();
                var responseDto = BlobDTOsMapping.MapToBlobListResponseDto(blobs);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
