using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class UpdateFcmTokenRequest
    {
        [Required]
        [StringLength(500, ErrorMessage = "FCM Token length can't exceed 500 characters.")]
        public string FcmToken { get; set; }
    }
}
