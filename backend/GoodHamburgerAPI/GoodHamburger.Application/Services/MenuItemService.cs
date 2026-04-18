using AutoMapper;
using GoodHamburger.Application.DTOs.Response;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public MenuItemService(IMenuItemRepository menuItemRepository,IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuItemResponse>> GetAllAsync()
        {
            var items = await _menuItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MenuItemResponse>>(items);
        }
    }
}

