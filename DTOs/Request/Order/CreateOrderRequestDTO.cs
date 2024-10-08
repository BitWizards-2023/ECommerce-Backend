namespace ECommerceBackend.DTOs.Request.Order
{
    public class CreateOrderRequestDTO
    {
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        public AddressDTO ShippingAddress { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
