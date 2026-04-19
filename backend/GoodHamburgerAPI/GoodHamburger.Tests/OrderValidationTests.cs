using FluentAssertions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Strategies;
using GoodHamburger.Tests.Helpers;

namespace GoodHamburger.Tests
{
    public class OrderValidationTests
    {
        private static OrderItem ToOrderItem(MenuItem menuItem)
            => new OrderItem(menuItem.Id, menuItem);

        [Fact(DisplayName = "Adicionar dois sanduiches deve lançar DuplicateItemTypeException")]
        public void AddItem_ShouldThrow_WhenTwoSandwiches()
        {
            var order = new Order(new NoDiscountStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich("X Burger")));

            var act = () => order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich("X Bacon")));

            act.Should().Throw<DuplicateItemTypeException>()
                .WithMessage("*Sandwich*");
        }

        [Fact(DisplayName = "Adicionar duas batatas deve lançar DuplicateItemTypeException")]
        public void AddItem_ShouldThrow_WhenTwoFries()
        {
            var order = new Order(new NoDiscountStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Fries()));

            var act = () => order.AddItem(ToOrderItem(MenuItemBuilder.Fries()));

            act.Should().Throw<DuplicateItemTypeException>()
                .WithMessage("*Fries*");
        }

        [Fact(DisplayName = "Adicionar dois refrigerantes deve lançar DuplicateItemTypeException")]
        public void AddItem_ShouldThrow_WhenTwoDrinks()
        {
            var order = new Order(new NoDiscountStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Drink()));

            var act = () => order.AddItem(ToOrderItem(MenuItemBuilder.Drink()));

            act.Should().Throw<DuplicateItemTypeException>()
                .WithMessage("*Drink*");
        }

        [Fact(DisplayName = "Pedido com itens únicos por tipo não deve lançar exceção")]
        public void AddItem_ShouldNotThrow_WhenItemsAreUnique()
        {
            var order = new Order(new FullComboStrategy());

            var act = () =>
            {
                order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich()));
                order.AddItem(ToOrderItem(MenuItemBuilder.Fries()));
                order.AddItem(ToOrderItem(MenuItemBuilder.Drink()));
            };

            act.Should().NotThrow();
        }

        [Fact(DisplayName = "A mensagem de erro de item duplicado deve ser clara")]
        public void AddItem_ErrorMessage_ShouldBeClear()
        {
            var order = new Order(new NoDiscountStrategy());
            order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich()));

            var act = () => order.AddItem(ToOrderItem(MenuItemBuilder.Sandwich("X Bacon")));

            act.Should().Throw<DuplicateItemTypeException>()
                .WithMessage("*apenas um sanduíche*");
        }
    }
}
