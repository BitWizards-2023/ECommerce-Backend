using System;

namespace ECommerceBackend.DTOs.Response.Upload;

public class DownloadFileResponseDto
{
    public string BlobName { get; set; }
    public string ContentType { get; set; }
    public Stream FileStream { get; set; }
}
