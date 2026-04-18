using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Strategies
{
    public interface IDiscountStrategy
    {
        decimal DiscountPercentage { get;}
        bool IsApplicable(IReadOnlyList<OrderItem> items);
    }
}
