namespace GoodHamburger.Application.DTOs.Response
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
