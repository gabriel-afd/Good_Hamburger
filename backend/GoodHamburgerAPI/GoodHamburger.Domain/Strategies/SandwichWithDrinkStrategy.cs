using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Strategies
{
    public class SandwichWithDrinkStrategy : IDiscountStrategy
    {
        public decimal DiscountPercentage => 0.15m;

        public bool IsApplicable(IReadOnlyList<OrderItem> items)
        {
            var types = items.Select(i => i.MenuItem.Type).ToHashSet();

            return types.Contains(Enums.ItemType.Sandwich)
                && types.Contains(Enums.ItemType.Drink)
                && !types.Contains(Enums.ItemType.Fries);
        }
    }
}
