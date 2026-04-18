using GoodHamburger.Application.DTOs.Response;

namespace GoodHamburger.Application.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemResponse>> GetAllAsync();
    }
}

