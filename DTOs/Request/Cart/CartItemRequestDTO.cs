namespace ECommerceBackend.DTOs.Request.Cart
{
    public class CartItemRequestDTO
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Dictionary<string, string>? SelectedOptions { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateCartItemRequestDTO
    {
        public int Quantity { get; set; }
        public Dictionary<string, string>? SelectedOptions { get; set; }
    }

    public class ApplyDiscountRequestDTO
    {
        public string DiscountCode { get; set; }
    }
}
