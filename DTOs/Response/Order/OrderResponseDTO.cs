namespace ECommerceBackend.DTOs.Response.Order
{
    public class OrderResponseDTO
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
