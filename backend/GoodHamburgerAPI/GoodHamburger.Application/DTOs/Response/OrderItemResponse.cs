namespace GoodHamburger.Application.DTOs.Response
{
    public class OrderItemResponse
    {
        public Guid Id { get; set; }
        public Guid MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal MenuItemPrice { get; set; }
    }
}
