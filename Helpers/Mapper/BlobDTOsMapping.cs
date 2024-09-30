/*
 * File: BlobDTOsMapping.cs
 * Description: Provides mapping methods to convert blob data to various response DTOs.
 * Author:Sudesh Sachintha Bandara
 * Date: 2024/09/30
 */


using ECommerceBackend.DTOs.Response;
using ECommerceBackend.DTOs.Response.Upload;

namespace ECommerceBackend.Helpers.Mapper
{
    public class BlobDTOsMapping
    {
        // Converts the provided blob URL into an UploadFileResponseDto object containing the file URL and blob name.
        public static UploadFileResponseDto MapToUploadFileResponseDto(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            return new UploadFileResponseDto { FileUrl = url, BlobName = GetBlobNameFromUrl(url) };
        }

        // Creates a DeleteFileResponseDto with an appropriate message based on the deletion status.
        public static DeleteFileResponseDto MapToDeleteFileResponseDto(bool isDeleted)
        {
            return new DeleteFileResponseDto
            {
                Message = isDeleted ? "File deleted successfully." : "File not found.",
            };
        }

        // Transforms a list of blob names into a BlobListResponseDto object, ensuring the list is not null.
        public static BlobListResponseDto MapToBlobListResponseDto(List<string> blobNames)
        {
            return new BlobListResponseDto { BlobNames = blobNames ?? new List<string>() };
        }

        // Parses the blob URL to extract and return the blob name.
        private static string GetBlobNameFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            return url.Split('/').Last();
        }
    }
}
