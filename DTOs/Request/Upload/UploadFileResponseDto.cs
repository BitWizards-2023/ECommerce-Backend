using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ECommerceBackend.DTOs.Request.Upload;

public class UploadFileRequestDto
{
    [Required(ErrorMessage = "File is required.")]
    public IFormFile File { get; set; }
}
