namespace GoodHamburger.Application.DTOs.Response
{
    public class MenuItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
