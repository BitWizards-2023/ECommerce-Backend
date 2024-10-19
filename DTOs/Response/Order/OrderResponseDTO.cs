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

        // New field for internal notes
        public List<OrderNoteDTO> InternalNotes { get; set; }
    }

    // Define DTO for order notes
    public class OrderNoteDTO
    {
        public string Note { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
