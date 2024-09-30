/*
 * File: BlobService.cs
 * Description: Implements blob storage operations such as upload, download, list, and delete using Azure Blob Storage.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-09-29
 */

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerceBackend.Service.Interfaces;

namespace ECommerceBackend.Service.Implementations
{
    public class BlobService : IBloblService
    {
        private readonly BlobContainerClient _blobContainerClient;

        // Initializes the BlobService with the necessary Azure Blob Storage configurations.
        public BlobService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureBlobStorage:ConnectionString"];
            string containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobContainerClient = new BlobContainerClient(connectionString, containerName);

            // Ensure the container exists; if not, create it with no public access.
            _blobContainerClient.CreateIfNotExists(PublicAccessType.None);
        }

        /// <summary>
        /// Deletes a specified blob asynchronously.
        /// </summary>
        /// <param name="blobName">The name of the blob to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains a boolean indicating success.</returns>
        // Deletes the specified blob from the Azure Blob Storage container.
        public async Task<bool> DeleteFileAsync(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name cannot be null or empty.", nameof(blobName));

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            return await blobClient.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Downloads a specified blob asynchronously.
        /// </summary>
        /// <param name="blobName">The name of the blob to download.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains the blob's content as a stream.</returns>
        // Downloads the specified blob and returns its content as a stream.
        public async Task<Stream> DownloadFileAsync(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name is null or empty.", nameof(blobName));

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();

            return response.Value.Content;
        }

        /// <summary>
        /// Lists all blobs in the container asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous list operation. The task result contains a list of blob names.</returns>
        // Retrieves a list of all blob names in the Azure Blob Storage container.
        public async Task<List<string>> ListBlobsAsync()
        {
            List<string> blobs = new List<string>();

            await foreach (BlobItem blobItem in _blobContainerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }

        /// <summary>
        /// Uploads a file to Azure Blob Storage asynchronously.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>A task that represents the asynchronous upload operation. The task result contains the URL of the uploaded blob.</returns>
        // Uploads the provided file to Azure Blob Storage and returns the blob's URL.
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.", nameof(file));

            // Generate a unique blob name using a GUID and preserve the file extension.
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Get a reference to the BlobClient for the generated blob name.
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

            // Upload the file using the BlobClient with the appropriate content type.
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(
                    stream,
                    new BlobHttpHeaders { ContentType = file.ContentType } // Set the content type of the blob.
                );
            }

            // Return the URL of the uploaded blob.
            return blobClient.Uri.ToString();
        }
    }
}
