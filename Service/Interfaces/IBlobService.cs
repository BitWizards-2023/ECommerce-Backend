/*
 * Author: Sudesh Sachintha Bandara
 * Description: This file defines the IBlobService interface, which provides
 * functionality for managing files in an Azure Blob Storage or similar storage system.
 * It includes methods for uploading, downloading, deleting, and listing blobs.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.Service.Interfaces
{
    public interface IBlobService
    {
        /// <summary>
        /// Uploads a file to the blob storage asynchronously.
        /// </summary>
        /// <param name="file">The file to be uploaded</param>
        /// <returns>The URL of the uploaded file</returns>
        Task<string> UploadFileAsync(IFormFile file);

        /// <summary>
        /// Downloads a file from the blob storage asynchronously.
        /// </summary>
        /// <param name="blobName">The name of the blob to be downloaded</param>
        /// <returns>A stream of the downloaded file</returns>
        Task<Stream> DownloadFileAsync(string blobName);

        /// <summary>
        /// Deletes a file from the blob storage asynchronously.
        /// </summary>
        /// <param name="blobName">The name of the blob to be deleted</param>
        /// <returns>True if the file was deleted successfully, false otherwise</returns>
        Task<bool> DeleteFileAsync(string blobName);

        /// <summary>
        /// Lists all blobs in the storage container asynchronously.
        /// </summary>
        /// <returns>A list of blob names in the storage container</returns>
        Task<List<string>> ListBlobsAsync();
    }
}
