using System.Runtime.InteropServices;
using AutoMapper;
using GoodHamburger.Application.DTOs.Request;
using GoodHamburger.Application.DTOs.Response;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Factory;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Strategies;

namespace GoodHamburger.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly OrderFactory _orderFactory;
        private readonly DiscountStrategyResolver _strategyResolver;
        private readonly IMapper _mapper;

        public OrderService(
           IOrderRepository orderRepository,
           IMenuItemRepository menuItemRepository,
           OrderFactory orderFactory,
           DiscountStrategyResolver strategyResolver,
           IMapper mapper)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _orderFactory = orderFactory;
            _strategyResolver = strategyResolver;
            _mapper = mapper;
        }
        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            var itemData = await ResolveMenuItemsAsync(request.MenuItemIds);

            var order = _orderFactory.Create(itemData);

            await _orderRepository.AddAsync(order);

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task DeleteAsync(Guid id)
        {
            var exists = await _orderRepository.GetByIdAsync(id);

            if (exists is null)
                throw new OrderNotFoundException(id);

            await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id) ?? throw new OrderNotFoundException(id);

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse> UpdateAsync(Guid id, UpdateOrderRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(id) ?? throw new OrderNotFoundException(id);

            var itemData = await ResolveMenuItemsAsync(request.MenuItemIds);

            var newItems = itemData
                .Select(i => new OrderItem(i.MenuItemId, i.MenuItem))
                .ToList();

            var newStrategy = _strategyResolver.Resolve(newItems.AsReadOnly());

            order.UpdateItems(newItems, newStrategy);

            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponse>(order);
        }

        private async Task<List<(MenuItem MenuItem, Guid MenuItemId)>> ResolveMenuItemsAsync(List<Guid> menuItemIds)
        {
            var result = new List<(MenuItem, Guid)>();

            foreach (var menuItemId in menuItemIds)
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId) ??
                    throw new DomainException($"Item do cardápio com ID '{menuItemId}' não encontrado.");

                result.Add((menuItem, menuItemId));
            }

            return result;
        }
    }
}
