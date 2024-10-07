using System;

namespace ECommerceBackend.DTOs.Request.SubmitRatings;

public class SubmitRatingRequestDTO
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
