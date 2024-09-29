using System;

namespace ECommerceBackend.DTOs.Response;

public class UploadFileResponseDto
{
    public string FileUrl { get; set; }
    public string BlobName { get; set; }
}
