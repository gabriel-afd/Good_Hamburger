using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Tests.Helpers
{
    public class MenuItemBuilder
    {
        public static MenuItem Sandwich(string name = "X Burger", decimal price = 5.00m) 
            => new MenuItem(name, price, ItemType.Sandwich);

        public static MenuItem Fries(decimal price = 2.00m)
            => new MenuItem("Batata Frita", price, ItemType.Fries);

        public static MenuItem Drink(decimal price = 2.50m)
            => new MenuItem("Refrigerante", price, ItemType.Drink);
    }
}
