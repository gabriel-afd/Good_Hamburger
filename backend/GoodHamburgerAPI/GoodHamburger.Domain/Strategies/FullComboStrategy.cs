using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Strategies
{
    public class FullComboStrategy : IDiscountStrategy
    {
        public decimal DiscountPercentage => 0.20m;

        public bool IsApplicable(IReadOnlyList<OrderItem> items)
        {
            var types = items.Select(i => i.MenuItem.Type).ToHashSet();

            return types.Contains(Enums.ItemType.Sandwich)
                && types.Contains(Enums.ItemType.Fries)
                && types.Contains(Enums.ItemType.Drink);
        }
    }
}
