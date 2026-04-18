namespace GoodHamburger.Application.DTOs.Request
{
    public class CreateOrderRequest
    {
        public List<Guid> MenuItemIds { get; set; } = new();
    }
}
