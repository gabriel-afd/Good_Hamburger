using FluentAssertions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Strategies;
using GoodHamburger.Tests.Helpers;

namespace GoodHamburger.Tests
{
    public class OrderCalculationTests
    {
        private static OrderItem ToOrderItem(MenuItem menuItem)
            => new OrderItem(menuItem.Id, menuItem);

        [Fact(DisplayName = "Combo completo deve calcular subtotal, desconto e total corretamente")]
        public void FullCombo_ShouldCalculate_Correctly()
        {
            var order = new Order(new FullComboStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich("X Bacon", 7.00m)));
            order.AddItem(ToOrderItem(MenuItemBuilder.Fries()));
            order.AddItem(ToOrderItem(MenuItemBuilder.Drink()));

            order.GetSubtotal().Should().Be(11.50m);
            order.DiscountPercentage.Should().Be(0.20m);
            order.GetDiscount().Should().Be(2.30m);
            order.GetTotal().Should().Be(9.20m);
        }

        [Fact(DisplayName = "Sanduiche + refrigerante deve calcular 15% de desconto corretamente")]
        public void SandwichWithDrink_ShouldCalculate_Correctly()
        {
            var order = new Order(new SandwichWithDrinkStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich("X Egg", 4.50m)));
            order.AddItem(ToOrderItem(MenuItemBuilder.Drink()));

            order.GetSubtotal().Should().Be(7.00m);
            order.GetDiscount().Should().Be(1.05m);
            order.GetTotal().Should().Be(5.95m);
        }

        [Fact(DisplayName = "Sanduiche + batata deve calcular 10% de desconto corretamente")]
        public void SandwichWithFries_ShouldCalculate_Correctly()
        {
            var order = new Order(new SandwichWithFriesStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich()));
            order.AddItem(ToOrderItem(MenuItemBuilder.Fries()));

            order.GetSubtotal().Should().Be(7.00m);
            order.GetDiscount().Should().Be(0.70m);
            order.GetTotal().Should().Be(6.30m);
        }

        [Fact(DisplayName = "Só sanduiche não deve ter desconto")]
        public void OnlySandwich_ShouldHave_NoDiscount()
        {
            var order = new Order(new NoDiscountStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich()));

            order.GetSubtotal().Should().Be(5.00m);
            order.GetDiscount().Should().Be(0m);
            order.GetTotal().Should().Be(5.00m);
        }
    }
}
