using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Strategies;

namespace GoodHamburger.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }


        private readonly List<OrderItem> _items = new();
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

        private IDiscountStrategy _discountStrategy;

        private Order() { _discountStrategy = new NoDiscountStrategy(); }

        public Order(IDiscountStrategy initialStrategy)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            _discountStrategy = initialStrategy;
        }

        public void AddItem(OrderItem item)
        {
            var alreadyExists = _items.Any(i => i.MenuItem.Type == item.MenuItem.Type);

            if (alreadyExists)
                throw new DuplicateItemTypeException(item.MenuItem.Type);

            _items.Add(item);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateItems(IEnumerable<OrderItem> newItems, IDiscountStrategy newStrategy)
        {
            _items.Clear();

            foreach (var item in newItems)
                AddItem(item);

            _discountStrategy = newStrategy;
            UpdatedAt = DateTime.UtcNow;
        }

        public decimal GetSubtotal() => _items.Sum(i => i.MenuItem.Price);

        public decimal GetDiscount() => Math.Round(GetSubtotal() * _discountStrategy.DiscountPercentage, 2);

        public decimal GetTotal() => GetSubtotal() - GetDiscount();

        public decimal GetDiscountPercentage() => _discountStrategy.DiscountPercentage;
    }
}
