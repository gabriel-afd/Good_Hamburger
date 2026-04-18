using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Strategies;

namespace GoodHamburger.Domain.Factory
{
    public class OrderFactory
    {
        private readonly DiscountStrategyResolver _resolver;

        public OrderFactory(DiscountStrategyResolver resolver)
        {
            _resolver = resolver;
        }

        public Order Create(IEnumerable<(MenuItem MenuItem, Guid MenuItemId)> itemData)
        {
            var itemList = itemData.ToList();

            if (!itemList.Any())
                throw new DomainException("O pedido deve conter pelo menos um item.");

            var orderItems = itemList
                .Select(i => new OrderItem(i.MenuItemId, i.MenuItem))
                .ToList();

            var strategy = _resolver.Resolve(orderItems.AsReadOnly());

            var order = new Order(strategy);

            foreach(var item in orderItems)
            {
                order.AddItem(item);
            }

            return order;
        }
    }
}
