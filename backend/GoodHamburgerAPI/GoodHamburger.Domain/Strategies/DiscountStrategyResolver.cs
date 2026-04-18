using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Strategies
{
    public class DiscountStrategyResolver
    {
        private readonly IEnumerable<IDiscountStrategy> _strategies;

        public DiscountStrategyResolver(IEnumerable<IDiscountStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IDiscountStrategy Resolve(IReadOnlyList<OrderItem> items)
        {
            return _strategies
                .OrderByDescending(s => s.DiscountPercentage)
                .First(s => s.IsApplicable(items));
        }
    }
}
