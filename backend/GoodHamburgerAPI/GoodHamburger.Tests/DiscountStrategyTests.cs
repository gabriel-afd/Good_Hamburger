using FluentAssertions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Strategies;
using GoodHamburger.Tests.Helpers;

namespace GoodHamburger.Tests
{
    public class DiscountStrategyTests
    {

        private static OrderItem ToOrderItem(MenuItem menuItem)
            => new OrderItem(menuItem.Id, menuItem);

        private static IReadOnlyList<OrderItem> Items(params MenuItem[] menuItems)
            => menuItems.Select(ToOrderItem).ToList().AsReadOnly();


        //FullComboStrategy — 20%
        [Fact(DisplayName = "Sanduiche + batata + refrigerante deve aplicar 20% de desconto")]
        public void FullCombo_ShouldApply_20PercentDiscount()
        {
            var items = Items(
                MenuItemBuilder.Sandwich(),
                MenuItemBuilder.Fries(),
                MenuItemBuilder.Drink()
            );

            var strategy = new FullComboStrategy();

            strategy.IsApplicable(items).Should().BeTrue();
            strategy.DiscountPercentage.Should().Be(0.20m);
        }

        [Fact(DisplayName = "Combo completo não deve ser aplicado sem drink")]
        public void FullCombo_ShouldNotApply_WithoutDrink()
        {
            var items = Items(MenuItemBuilder.Sandwich(), MenuItemBuilder.Fries());

            var strategy = new FullComboStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }

        [Fact(DisplayName = "Combo completo não deve ser aplicado sem batata")]
        public void FullCombo_ShouldNotApply_WithoutFries()
        {
            var items = Items(MenuItemBuilder.Sandwich(), MenuItemBuilder.Drink());

            var strategy = new FullComboStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }

        [Fact(DisplayName = "Combo completo não deve ser aplicado sem sanduíche")]
        public void FullCombo_ShouldNotApply_WithoutSandwich()
        {
            var items = Items(MenuItemBuilder.Fries(), MenuItemBuilder.Drink());

            var strategy = new FullComboStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }


        // SandwichWithDrinkStrategy — 15%
        [Fact(DisplayName = "Sanduiche + refrigerante deve aplicar 15% de desconto")]
        public void SandwichWithDrink_ShouldApply_15PercentDiscount()
        {
            var items = Items(MenuItemBuilder.Sandwich(), MenuItemBuilder.Drink());

            var strategy = new SandwichWithDrinkStrategy();

            strategy.IsApplicable(items).Should().BeTrue();
            strategy.DiscountPercentage.Should().Be(0.15m);
        }

        [Fact(DisplayName = "Sanduiche + refrigerante + batata não deve aplicar desconto de 15% (combo completo tem prioridade)")]
        public void SandwichWithDrink_ShouldNotApply_WhenFriesIsPresent()
        {
            var items = Items(
                MenuItemBuilder.Sandwich(),
                MenuItemBuilder.Drink(),
                MenuItemBuilder.Fries()
            );

            var strategy = new SandwichWithDrinkStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }

        [Fact(DisplayName = "Só refrigerante não deve aplicar desconto de 15%")]
        public void SandwichWithDrink_ShouldNotApply_WithoutSandwich()
        {
            var items = Items(MenuItemBuilder.Drink());

            var strategy = new SandwichWithDrinkStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }


        // SandwichWithFriesStrategy — 10%
        [Fact(DisplayName = "Sanduiche + batata deve aplicar 10% de desconto")]
        public void SandwichWithFries_ShouldApply_10PercentDiscount()
        {
            var items = Items(MenuItemBuilder.Sandwich(), MenuItemBuilder.Fries());

            var strategy = new SandwichWithFriesStrategy();

            strategy.IsApplicable(items).Should().BeTrue();
            strategy.DiscountPercentage.Should().Be(0.10m);
        }

        [Fact(DisplayName = "Sanduiche + batata + refrigerante não deve aplicar desconto de 10% (combo completo tem prioridade)")]
        public void SandwichWithFries_ShouldNotApply_WhenDrinkIsPresent()
        {
            var items = Items(
                MenuItemBuilder.Sandwich(),
                MenuItemBuilder.Fries(),
                MenuItemBuilder.Drink()
            );

            var strategy = new SandwichWithFriesStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }

        [Fact(DisplayName = "Só batata não deve aplicar desconto de 10%")]
        public void SandwichWithFries_ShouldNotApply_WithoutSandwich()
        {
            var items = Items(MenuItemBuilder.Fries());

            var strategy = new SandwichWithFriesStrategy();

            strategy.IsApplicable(items).Should().BeFalse();
        }


        // NoDiscountStrategy — 0%
        [Fact(DisplayName = "Só sanduiche não deve aplicar desconto")]
        public void NoDiscount_ShouldApply_WhenOnlySandwich()
        {
            var items = Items(MenuItemBuilder.Sandwich());

            var strategy = new NoDiscountStrategy();

            strategy.IsApplicable(items).Should().BeTrue();
            strategy.DiscountPercentage.Should().Be(0m);
        }


        // DiscountStrategyResolver — escolhe a melhor strategy
        [Fact(DisplayName = "Resolver deve escolher 20% para combo completo")]
        public void Resolver_ShouldChoose_FullCombo()
        {
            var items = Items(
                MenuItemBuilder.Sandwich(),
                MenuItemBuilder.Fries(),
                MenuItemBuilder.Drink()
            );

            var resolver = BuildResolver();
            var strategy = resolver.Resolve(items);

            strategy.DiscountPercentage.Should().Be(0.20m);
        }

        [Fact(DisplayName = "Resolver deve escolher 15% para sanduíche + refrigerante")]
        public void Resolver_ShouldChoose_SandwichWithDrink()
        {
            var items = Items(MenuItemBuilder.Sandwich(), MenuItemBuilder.Drink());

            var resolver = BuildResolver();
            var strategy = resolver.Resolve(items);

            strategy.DiscountPercentage.Should().Be(0.15m);
        }

        [Fact(DisplayName = "Resolver deve escolher 10% para sanduíche + batata")]
        public void Resolver_ShouldChoose_SandwichWithFries()
        {
            var items = Items(MenuItemBuilder.Sandwich(), MenuItemBuilder.Fries());

            var resolver = BuildResolver();
            var strategy = resolver.Resolve(items);

            strategy.DiscountPercentage.Should().Be(0.10m);
        }

        [Fact(DisplayName = "Resolver deve escolher 0% quando não há combo")]
        public void Resolver_ShouldChoose_NoDiscount()
        {
            var items = Items(MenuItemBuilder.Sandwich());

            var resolver = BuildResolver();
            var strategy = resolver.Resolve(items);

            strategy.DiscountPercentage.Should().Be(0m);
        }

        private static DiscountStrategyResolver BuildResolver() =>
            new DiscountStrategyResolver(new IDiscountStrategy[]
            {
            new FullComboStrategy(),
            new SandwichWithDrinkStrategy(),
            new SandwichWithFriesStrategy(),
            new NoDiscountStrategy()
            });
    }
}
