using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTOs.Request.Upload;

public class UploadFileRequestDto
{
    [Required(ErrorMessage = "File is required.")]
    public IFormFile File { get; set; }
}
