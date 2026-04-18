using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Strategies
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal DiscountPercentage => 0m;

        public bool IsApplicable(IReadOnlyList<OrderItem> items) => true;
    }
}
