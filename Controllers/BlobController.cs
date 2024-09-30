using ECommerceBackend.DTOs.Request.Upload;
using ECommerceBackend.Helpers.Mapper;
using ECommerceBackend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IBloblService _blobService;
        private readonly ILogger<BlobController> _logger;

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
            if (uploadDto.File == null || uploadDto.File.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
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
            if (string.IsNullOrWhiteSpace(blobName))
                return BadRequest("Blob name is required.");

            try
            {
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
            if (string.IsNullOrWhiteSpace(blobName))
                return BadRequest("Blob name is required.");

            try
            {
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
