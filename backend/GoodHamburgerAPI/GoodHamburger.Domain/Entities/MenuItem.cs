using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ItemType Type { get; set; }
        private MenuItem() { }

        public MenuItem(string name, decimal price, ItemType type)
        {
            ValidateName(name);
            ValidatePrice(price);

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Type = type;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do item não pode ser vazio.");
            
        }

        private static void ValidatePrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("O preço do item deve ser maior que zero.");
        }

    }
}
