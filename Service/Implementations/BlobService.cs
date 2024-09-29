using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerceBackend.Service.Interfaces;

namespace ECommerceBackend.Service.Implementations;

public class BlobService : IBloblService
{
    private readonly BlobContainerClient _blobContainerClient;

    public BlobService(IConfiguration configuration)
    {
        string connectionString = configuration["AzureBlobStorage:ConnectionString"];
        string containerName = configuration["AzureBlobStorage:ContainerName"];
        _blobContainerClient = new BlobContainerClient(connectionString, containerName);

        // Ensure the container exists
        _blobContainerClient.CreateIfNotExists(PublicAccessType.None);
    }

    Task<bool> IBloblService.DeleteFileAsync(string blobName)
    {
        throw new NotImplementedException();
    }

    async Task<Stream> IBloblService.DownloadFileAsync(string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new ArgumentException("Blob name is null or empty.", nameof(blobName));

        BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync();

        return response.Value.Content;
    }

    async Task<List<string>> IBloblService.ListBlobsAsync()
    {
        List<string> blobs = new List<string>();

        await foreach (BlobItem blobItem in _blobContainerClient.GetBlobsAsync())
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }

    async Task<string> IBloblService.UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is null or empty.", nameof(file));

        // Generate a unique blob name using a GUID
        string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Get a reference to the BlobClient
        BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

        // Upload the file using the BlobClient
        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = file.ContentType } // Set the content type of the blob
            );
        }

        // Return the URL of the uploaded blob
        return blobClient.Uri.ToString();
    }
}
