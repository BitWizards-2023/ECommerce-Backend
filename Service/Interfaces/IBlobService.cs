using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerceBackend.Service.Interfaces;

public interface IBloblService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<Stream> DownloadFileAsync(string blobName);
    Task<bool> DeleteFileAsync(string blobName);
    Task<List<string>> ListBlobsAsync();
}
