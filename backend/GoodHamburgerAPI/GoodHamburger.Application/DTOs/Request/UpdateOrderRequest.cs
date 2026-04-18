namespace GoodHamburger.Application.DTOs.Request
{
    public class UpdateOrderRequest
    {
        public List<Guid> MenuItemIds { get; set; } = new();
    }
}
