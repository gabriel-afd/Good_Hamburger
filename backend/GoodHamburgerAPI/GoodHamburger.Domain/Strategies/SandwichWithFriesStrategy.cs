using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Strategies
{
    public class SandwichWithFriesStrategy : IDiscountStrategy
    {
        public decimal DiscountPercentage => 0.10m;

        public bool IsApplicable(IReadOnlyList<OrderItem> items)
        {
            var types = items.Select(i => i.MenuItem.Type).ToHashSet();

            return types.Contains(Enums.ItemType.Sandwich)
                && types.Contains(Enums.ItemType.Fries)
                && !types.Contains(Enums.ItemType.Drink);
        }
    }
}
